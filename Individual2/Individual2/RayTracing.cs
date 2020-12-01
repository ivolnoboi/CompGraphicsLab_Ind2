using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics.Eventing.Reader;

namespace Individual2
{
    class RayTracing
    {
        private static float eps = 0.00001f;
        // Скалярное произведение
        public static float ScalarProduct(Point3D vector1, Point3D vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        // Длина вектора
        public static float Lenght(Point3D vec)
        {
            return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
        }

        // Приводит к вектору единичной длины
        public static Point3D Normalize(Point3D vec)
        {
            return vec / Lenght(vec);
        }

        // Получить координаты на Bitmap
        public static (int, int) GetCoordinatesBitmap(int x, int y, int width, int height)
        {
            return (width / 2 + x, height / 2 - y);
        }

        // Даёт корректный цвет (в диапазоне от 0 до 255)
        public static int GetCorrectValue(int value)
        {
            return Math.Min(255, Math.Max(0, value));
        }

        // Умножение цвета на коэффициент
        public static Color colorProd(Color color, float value)
        {
            var red = (int)(color.R * value);
            var green = (int)(color.G * value);
            var blue = (int)(color.B * value);
            return Color.FromArgb(GetCorrectValue(red), GetCorrectValue(green), GetCorrectValue(blue));
        }

        // Сложение цветов
        public static Color colorSum(Color color1, Color color2)
        {
            var red = color1.R + color2.R;
            var green = color1.G + color2.G;
            var blue = color1.B + color2.B;
            return Color.FromArgb(GetCorrectValue(red), GetCorrectValue(green), GetCorrectValue(blue));
        }

        // Заполняет буфер цвета
        public static Bitmap CreateColorScene(int width, int height, List<Figure> scene, List<Light> lights)
        {
            Bitmap colors = new Bitmap(width, height);
            Point3D obzor = new Point3D(0, 0, 0);
            for (int x = -width / 2 + 1; x < width / 2; x++)
                for (int y = -height / 2 + 1; y < height / 2; y++)
                {
                    Point3D D = CanvasToViewport(x, y, width, height);
                    Color color = TraceRay(obzor, D, 1, float.MaxValue, 5, scene, lights); // 3 - глубина рекурсии
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
        
        // Принадлежит ли точка данной грани
        private static bool pointIsInPlane(Plane plain, Point3D point)
        {
            return (point.X - Math.Max(plain.MaxPoint.X, plain.MinPoint.X) <= eps) && (point.X - Math.Min(plain.MaxPoint.X, plain.MinPoint.X) >= -eps)
                && (point.Y - Math.Max(plain.MaxPoint.Y, plain.MinPoint.Y) <= eps) && (point.Y - Math.Min(plain.MaxPoint.Y, plain.MinPoint.Y) >= -eps)
                && (point.Z - Math.Max(plain.MaxPoint.Z, plain.MinPoint.Z) <= eps) && (point.Z - Math.Min(plain.MaxPoint.Z, plain.MinPoint.Z) >= -eps);
        }

        // P - точка сцены, N - нормаль к поверхности
        // Высчитывает освещённость точки
        // specular - значение зеркальности
        // V - вектор обзора, указывающий из P в камеру
        public static float ComputeLighting(Point3D P, Point3D N, Point3D V, int specular, List<Figure> scene, List<Light> lights)
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
                    var tuple = elemIntersection(P, L, 0.001f, t_max, scene);
                    Figure shadow_elem = tuple.Item1;
                    float shadow_t = tuple.Item2;
                    if (shadow_elem != null && shadow_elem.type != ElemType.Plane)
                        continue;

                    // Диффузность
                    float scalar = ScalarProduct(N, L);
                    if (scalar > 0)
                        i += (float)(light.Intensity * scalar / (Lenght(N) * Lenght(L)));

                    // Зеркальность
                    if (specular != -1)
                    {
                        Point3D R = ReflectRay(N, L);
                        float scalarRV = ScalarProduct(R, V);
                        if (scalarRV > 0)
                            i += (float)(light.Intensity * Math.Pow(scalarRV / (Lenght(R) * Lenght(V)), specular));

                    }
                }
            }
            return i;
        }

        public static Color ReflectiveColor(Color local, Color reflective, float coefficient)
        {
            Color color1 = colorProd(local, 1 - coefficient);
            Color color2 = colorProd(reflective, coefficient);
            return colorSum(color1, color2);
        }

        // O - исходня точка луча, D - координата окна просмотра (лучи пускаются из O в D)
        // Вычисляет пересечение луча с каждой сферой, и возвращает цвет сферы в ближайшей точке пересечения, 
        // которая находится в требуемом интервале t (от 1 до бесконечности)
        public static Color TraceRay(Point3D O, Point3D D, float t_min, float t_max, int recursion_depth, List<Figure> scene, List<Light> lights)
        {
            var tuple = elemIntersection(O, D, t_min, t_max, scene);
            Figure closest_elem = tuple.Item1;
            float closest_t = tuple.Item2;

            if (closest_elem == null)
                return Color.Black;
            else
            {
                Point3D P = O + closest_t * D; // вычисление пересечения
                Point3D N;
                if (closest_elem.type == ElemType.Sphere)
                {
                    Sphere sphere = (Sphere)closest_elem;
                    N = P - sphere.Center; // вычисление нормали сферы в точке пересечения
                }
                else
                {
                    Plane plane = (Plane)closest_elem;
                    N = plane.Normal;
                }
                N = N / Lenght(N); // нормализуем вектор нормали
                float lightning = ComputeLighting(P, N, -D, closest_elem.Specular, scene, lights);
                Color local_color = colorProd(closest_elem.Color, lightning);

                // Если мы достигли предела рекурсии или объект не отражающий, то мы закончили
                float r = closest_elem.Reflective;
                if (recursion_depth <= 0 || r <= 0)
                    return local_color;

                // Вычисление отражённого цвета
                Point3D R = ReflectRay(N, -D);
                Color reflected_color = TraceRay(P, R, 0.001f, float.MaxValue, recursion_depth - 1, scene, lights);
                return ReflectiveColor(local_color, reflected_color, r);
            }
        }

        // Отраженный луч
        public static Point3D ReflectRay(Point3D Normal, Point3D Ray)
        {
            return 2 * Normal * ScalarProduct(Normal, Ray) - Ray;
        }

        // Находит элемент сцены, который пересекает луч, и параметр t, определяющий точку пересечения, в интервале от t_min до t_max
        public static (Figure, float) elemIntersection(Point3D viewPoint, Point3D direction, float t_min, float t_max, List<Figure> scene)
        {
            float intersection_t = float.MaxValue;
            Figure intersection_elem = null;
            foreach (var elem in scene)
            {
                switch (elem.type)
                {
                    case ElemType.Sphere:
                        Sphere sphere = (Sphere)elem;
                        ValueTuple<float, float> t = IntersectRaySphere(viewPoint, direction, sphere);
                        float t1 = t.Item1;
                        float t2 = t.Item2;
                        if (t1 > t_min && t1 < t_max && t1 < intersection_t)
                        {
                            intersection_t = t1;
                            intersection_elem = sphere;
                        }
                        if (t2 > t_min && t2 < t_max && t2 < intersection_t)
                        {
                            intersection_t = t2;
                            intersection_elem = sphere;
                        }
                        break;
                    case ElemType.Cube:
                        break;
                    case ElemType.Plane:
                        Plane plane = (Plane)elem;
                        float t_plane = IntersectRayPlane(viewPoint, direction, plane.MaxPoint, plane);
                        Point3D intersectionPoint = viewPoint + t_plane * direction; // точка пересечения на плоскости
                        if (pointIsInPlane(plane, intersectionPoint))
                        {
                            if (t_plane > t_min && t_plane < t_max && t_plane < intersection_t)
                            {
                                intersection_t = t_plane;
                                intersection_elem = plane;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return (intersection_elem, intersection_t);
        }

        // Находит параметр t для нахождения точки пересечения со сферой (решает квадратное уравнение)
        public static (float, float) IntersectRaySphere(Point3D viewPoint, Point3D direction, Sphere sphere)
        {
            Point3D center = sphere.Center;
            float radius = sphere.Radius;
            Point3D OC = viewPoint - center;

            float k1 = ScalarProduct(direction, direction); // коэффициент a при квадратном уравнении
            float k2 = 2 * ScalarProduct(OC, direction); // коэффициент b при квадратном уравнении
            float k3 = ScalarProduct(OC, OC) - radius * radius; // коэффициент c при квадратном уравнении

            float discriminant = k2 * k2 - 4 * k1 * k3;

            if (discriminant < 0) // если нет пересечений
                return (float.MaxValue, float.MaxValue);

            float t1 = (float)((-k2 + Math.Sqrt(discriminant)) / (2 * k1));
            float t2 = (float)((-k2 - Math.Sqrt(discriminant)) / (2 * k1));
            return (t1, t2);
        }

        // Находит параметр t для нахождения точки пересечения с плоскостью
        public static float IntersectRayPlane(Point3D viewPoint, Point3D direction, Point3D planePoint, Plane plane)
        {
            float t = float.MaxValue; // параметр для точки пересечения
            Point3D direct = direction - viewPoint; // ТУТ НЕЧТО СТРАННОЕ ПРОИСХОДИТ (ЕСЛИ ПРОСТО direction В СКАЛЯРНОЕ, ТО ВСЁ ЗЕРКАЛЬНОЕ СТАНОВИТСЯ)
            float denominator = ScalarProduct(plane.Normal, direction);
            if (denominator > 0)
            {
                Point3D vec = planePoint - viewPoint;
                float numerator = ScalarProduct(vec, plane.Normal);
                t = numerator / denominator;
            }
            return t;
        }
    }
}
