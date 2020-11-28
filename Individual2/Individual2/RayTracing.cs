using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Individual2
{
    class RayTracing
    {
        // Скалярное произведение
        public static float ScalarProduct(Point3D vector1, Point3D vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;

        // Длина вектора
        public static float Lenght(Point3D vec)
        {
            return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
        }

        // Получить координаты на Bitmap
        public static (int, int) GetCoordinatesBitmap(int x, int y, int width, int height)
        {
            return (width / 2 + x, height / 2 - y);
        }

        /// Заполняет буфер цвета
        public static Bitmap CreateColorScene(int width, int height, List<Sphere> scene, List<Light> lights)
        {
            Bitmap colors = new Bitmap(width, height);
            Camera obzor = new Camera(0, 0, 0);
            for (int x = -width / 2 + 1; x < width / 2; x++)
                for (int y = -height / 2 + 1; y < height / 2; y++)
                {
                    Point3D D = CanvasToViewport(x, y, width, height);
                    Color color = TraceRay(obzor, D, 1, float.MaxValue, scene, lights);
                    (int, int) coords = GetCoordinatesBitmap(x, y, width, height);
                    colors.SetPixel(coords.Item1, coords.Item2, color);
                }
            return colors;
        }

        // Преобразовать координаты холста в координаты окна просмотра (плоскости проекции)
        public static Point3D CanvasToViewport(int x, int y, int width, int height)
        {
            return new Point3D((float)(x * 1.0 / width), (float)(y * 1.0 / height), 1);
        }

        // P - точка сцены, N - нормаль к поверхности
        // Высчитывает освещённость точки
        // specular - значение зеркальности
        // V - вектор обзора, указывающий из P в камеру
        public static float ComputeLighting(Point3D P, Point3D N, Point3D V, int specular, List<Sphere> scene, List<Light> lights)
        {
            float i = 0.0f;
            float t_max = 0.0f;
            foreach (var light in lights)
            {
                if (light.Type == Type.Ambient)
                {
                    i += light.Intensity;
                }
                else
                {
                    Point3D L;
                    if (light.Type == Type.Point)
                    {
                        L = light.Position - P; // вектор освещения
                        t_max = 1;
                    }
                    else
                    {
                        L = light.Direction;
                        t_max = float.MaxValue;
                    }

                    // Проверка тени
                    var tuple = ClosestIntersection(new Camera(P), L, 0.001f, t_max, scene);
                    Sphere shadow_sphere = tuple.Item1;
                    float shadow_t = tuple.Item2;
                    if (shadow_sphere != null)
                        continue;

                    // Диффузность
                    float scalar = ScalarProduct(N, L);
                    if (scalar > 0)
                        i += (float)(light.Intensity * scalar / (Lenght(N) * Lenght(L)));

                    // Зеркальность
                    if (specular != -1)
                    {
                        Point3D R = 2 * N * ScalarProduct(N, L) - L;
                        float scalarRV = ScalarProduct(R, V);
                        if (scalarRV > 0)
                            i += (float)(light.Intensity * Math.Pow(scalarRV / (Lenght(R) * Lenght(V)), specular));

                    }
                }
            }
            return i;
        }

        public static Color colorWithLightning(Color color, float lightning)
        {
            var red = (int)(color.R * lightning);
            var green = (int)(color.G * lightning);
            var blue = (int)(color.B * lightning);
            return Color.FromArgb(Math.Min(255, Math.Max(0, red)), Math.Min(255, Math.Max(0, green)), Math.Min(255, Math.Max(0, blue)));
        }

        // O - исходня точка луча, D - координата окна просмотра (лучи пускаются из O в D)
        // Вычисляет пересечение луча с каждой сферой, и возвращает цвет сферы в ближайшей точке пересечения, 
        // которая находится в требуемом интервале t (от 1 до бесконечности)
        public static Color TraceRay(Camera O, Point3D D, float t_min, float t_max, List<Sphere> scene, List<Light> lights)
        {
            var tuple = ClosestIntersection(O, D, t_min, t_max, scene);
            Sphere closest_sphere = tuple.Item1;
            float closest_t = tuple.Item2;
            if (closest_sphere == null)
                return Color.White;
            else
            {
                Point3D P = O.Position + closest_t * D; // вычисление пересечения
                Point3D N = P - closest_sphere.Center; // вычисление нормали сферы в точке пересечения
                N = N / Lenght(N); // нормализуем вектор нормали
                float lightning = ComputeLighting(P, N, -D, closest_sphere.Specular, scene, lights);
                return colorWithLightning(closest_sphere.Color, lightning);
            }
        }

        public static (Sphere, float) ClosestIntersection(Camera O, Point3D D, float t_min, float t_max, List<Sphere> scene)
        {
            float closest_t = float.MaxValue;
            Sphere closest_sphere = null;
            foreach (var sphere in scene)
            {
                ValueTuple<float, float> t = IntersectRaySphere(O, D, sphere);
                float t1 = t.Item1;
                float t2 = t.Item2;
                if (t1 > t_min && t1 < t_max && t1 < closest_t)
                {
                    closest_t = t1;
                    closest_sphere = sphere;
                }
                if (t2 > t_min && t2 < t_max && t2 < closest_t)
                {
                    closest_t = t2;
                    closest_sphere = sphere;
                }
            }
            return (closest_sphere, closest_t);
        }

        // Находит параметр t для нахождения точки пересечения со сферой
        public static (float, float) IntersectRaySphere(Camera O, Point3D D, Sphere sphere)
        {
            Point3D C = sphere.Center;
            float r = sphere.Radius;
            Point3D OC = O.Position - C;

            float k1 = ScalarProduct(D, D);
            float k2 = 2 * ScalarProduct(OC, D);
            float k3 = ScalarProduct(OC, OC) - r * r;

            float discriminant = k2 * k2 - 4 * k1 * k3;
            if (discriminant < 0)
                return (float.MaxValue, float.MaxValue);

            float t1 = (float)((-k2 + Math.Sqrt(discriminant)) / (2 * k1));
            float t2 = (float)((-k2 - Math.Sqrt(discriminant)) / (2 * k1));
            return (t1, t2);
        }
    }
}
