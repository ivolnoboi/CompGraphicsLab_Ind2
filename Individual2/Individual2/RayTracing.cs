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

        // Получить координаты на Bitmap
        public static (int, int) GetCoordinatesBitmap(int x, int y, int width, int height)
        {
            return (width / 2 + x, height / 2 - y);
        }

        /// Заполняет буфер цвета
        public static Bitmap CreateColorScene(int width, int height, List<Sphere> scene)
        {
            Bitmap colors = new Bitmap(width, height);
            Camera obzor = new Camera(0, 0, 0);
            for (int x = -width / 2 + 1; x < width / 2; x++)
                for (int y = -height / 2 + 1; y < height / 2; y++)
                {
                    Point3D D = CanvasToViewport(x, y, width, height);
                    Color color = TraceRay(obzor, D, 1, float.MaxValue, scene);
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

        // O - исходня точка луча, D - координата окна просмотра (лучи пускаются из O в D)
        // Вычисляет пересечение луча с каждой сферой, и возвращает цвет сферы в ближайшей точке пересечения, 
        // которая находится в требуемом интервале t (от 1 до бесконечности)
        public static Color TraceRay(Camera O, Point3D D, float t_min, float t_max, List<Sphere> scene)
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
            if (closest_sphere == null)
                return Color.White;
            else return closest_sphere.Color;
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
