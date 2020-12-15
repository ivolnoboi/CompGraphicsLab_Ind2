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
        private static double eps = 0.0001f;
        private static List<Figure> Scene;
        private static List<Light> Lights;

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        public static double ScalarProduct(Point3D vector1, Point3D vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        /// <summary>
        /// Длина вектора
        /// </summary>
        public static double Lenght(Point3D vec)
        {
            return Math.Sqrt(ScalarProduct(vec, vec));
        }

        /// <summary>
        /// Приводит к вектору единичной длины
        /// </summary>
        public static Point3D Normalize(Point3D vec)
        {
            return vec / Lenght(vec);
        }

        /// <summary>
        /// Получить координаты на Bitmap
        /// </summary>
        public static (int, int) GetCoordinatesBitmap(int x, int y, int width, int height)
        {
            return (width / 2 + x, height / 2 - y);
        }

        /// <summary>
        /// Преобразовать координаты холста в координаты окна просмотра (плоскости проекции)
        /// </summary>
        public static Point3D CanvasToViewport(int x, int y, int width, int height)
        {
            double viewWindowWidth = 1;
            double viewWindowHeight = 1;
            double ditanceFromCamera = 1.0;
            double X = x * viewWindowWidth / width;
            double Y = y * viewWindowHeight / height;
            return new Point3D(X, Y, ditanceFromCamera);
        }

        // Даёт корректное значение (которое должно лежать в диапазоне [min, max])
        public static int GetCorrectValue(int value, int min = 0, int max = 255)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        // Даёт корректное значение (которое должно лежать в диапазоне [min, max])
        public static double GetCorrectValue(double value, double min = 0, double max = 255)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        // Умножение цвета на коэффициент
        public static Color colorProd(Color color, double value)
        {
            int red = (int)(color.R * value + 5);
            int green = (int)(color.G * value + 5);
            int blue = (int)(color.B * value + 5);
            return Color.FromArgb(GetCorrectValue(red), GetCorrectValue(green), GetCorrectValue(blue));
        }

        // Сложение цветов
        public static Color colorSum(Color color1, Color color2)
        {
            int red = color1.R + color2.R;
            int green = color1.G + color2.G;
            int blue = color1.B + color2.B;
            return Color.FromArgb(GetCorrectValue(red), GetCorrectValue(green), GetCorrectValue(blue));
        }

        /// <summary>
        /// Заполняет буфер цвета
        /// </summary>
        public static Bitmap CreateColorScene(int width, int height, List<Figure> scene, List<Light> lights)
        {
            Scene = scene;
            Lights = lights;
            Point3D camera = new Point3D(0.0, 4.0, -20.0);

            Color[,] colors1 = new Color[height + 1, width + 1];
            for (int y = (-height / 2) + 1; y < height / 2; y++)
                Parallel.For((-width / 2) + 1, width / 2, x =>
               {
                   Point3D D = CanvasToViewport(x, y, width, height);
                   Color color = TraceRay(camera, D, 1.0, double.MaxValue, 1);
                   var coord = GetCoordinatesBitmap(x, y, width, height);
                   if (!(coord.Item1 < 0 || coord.Item1 >= width || coord.Item2 < 0 || coord.Item2 >= height))
                       colors1[y + height / 2, x + height / 2] = color;
               });

            Bitmap bmp = new Bitmap(width, height);
            for (int y = (-height / 2) + 1; y < height / 2; y++)
                for (int x = (-width / 2) + 1; x < width / 2; x++)
                {
                    var coord = GetCoordinatesBitmap(x, y, width, height);
                    bmp.SetPixel(coord.Item1, coord.Item2, colors1[y + height / 2, x + height / 2]);
                }
            return bmp;
        }

        /// <summary>
        /// Вычисляет освещенность точки
        /// </summary>
        private static double computeLighting(Point3D point, Point3D normal, Point3D view, double specular)
        {
            double intensity = 0;

            foreach (var light in Lights)
            {
                    Point3D vectorLight = light.Position - point;
                    double t = 1.0;

                    var (figureShadow, _, _) = Intersection(point, vectorLight, eps, t);
                    if (figureShadow != null)
                        continue;

                    double scalarNL = ScalarProduct(normal, vectorLight);
                    if (scalarNL > 0)
                    {
                        intensity += light.Intensity * scalarNL / (Lenght(normal) * Lenght(vectorLight));
                    }

                    if (specular > 0)
                    {
                        Point3D vectorReflect = ReflectRay(vectorLight, normal);
                        double scalarRV = ScalarProduct(vectorReflect, view);
                        if (scalarRV > 0)
                        {
                            intensity += light.Intensity * Math.Pow(scalarRV / (Lenght(vectorReflect) * Lenght(view)), specular);
                        }
                    }
            }
            return intensity;
        }

        public static Color ProdSumColor(Color local, Color reflect_refractColor, double coefficient)
        {
            Color color1 = colorProd(local, 1 - coefficient);
            Color color2 = colorProd(reflect_refractColor, coefficient);
            return colorSum(color1, color2);
        }

        private static Color TraceRay(Point3D camera, Point3D D, double t_min, double t_max, double intensityRay, int recurseDepth = 5)
        {
            var (closest_elem, closest_t, normal) = Intersection(camera, D, t_min, t_max);

            if (closest_elem == null)
                return Color.White;

            normal = Normalize(normal);

            Point3D point = camera + closest_t * D; // точка пересечения

            double light = computeLighting(point, normal, -D, closest_elem.Material.Specular);
            Color localColor = colorProd(closest_elem.Color, light);

            if (recurseDepth <= 0 || intensityRay <= eps)
                return localColor;

            Point3D reflectedRay = ReflectRay(-D, normal);
            Color reflectionColor = TraceRay(point, reflectedRay, eps, double.MaxValue, intensityRay * closest_elem.Material.Reflective, recurseDepth - 1);
            Color reflected = ProdSumColor(localColor, reflectionColor, closest_elem.Material.Reflective);

            if (closest_elem.Material.Transparent <= 0)
                return colorProd(reflected, intensityRay);

            Point3D refractedRay = RefractRay(D, normal, 1.5);
            Color transparentColor = TraceRay(point, refractedRay, eps, double.MaxValue, intensityRay * closest_elem.Material.Transparent, recurseDepth - 1);
            Color transparent = ProdSumColor(reflected, transparentColor, closest_elem.Material.Transparent);

            return colorProd(transparent, intensityRay);
        }

        /// <summary>
        /// Расчет преломленного луча
        /// </summary>
        private static Point3D RefractRay(Point3D Ray, Point3D Normal, double coefElem = 1.102, double coefAir = 1.0)
        {
            var cos = GetCorrectValue(ScalarProduct(Ray, Normal), -1.0, 1.0);
            Normal = cos < 0 ? Normal : -Normal;
            var coef = cos < 0 ? coefAir / coefElem : coefElem / coefAir;

            if (cos < 0)
                cos = -cos;

            var k = 1.0 - coef * coef * (1.0 - cos * cos);
            return (k < 0) ? new Point3D(0, 0, 0) : coef * Ray + (coef * cos - Math.Sqrt(k)) * Normal;
        }


        /// <summary>
        /// Расчёт отраженного луча
        /// </summary>
        public static Point3D ReflectRay(Point3D Ray, Point3D Normal)
        {
            return 2 * Normal * ScalarProduct(Normal, Ray) - Ray;
        }

        /// <summary>
        /// Находим пересечение луча с элементом сцены
        /// </summary>
        private static (Figure, double, Point3D) Intersection(Point3D camera, Point3D D, double t_min, double t_max)
        {
            Figure closest_figure = null;
            double closest_t = double.MaxValue;
            Point3D normal = null;
            foreach (var elem in Scene)
            {
                switch (elem.type)
                {
                    case ElemType.Sphere:
                        var t = IntersectRaySphere(camera, D, (Sphere)elem);
                        double t1 = t.Item1, t2 = t.Item2;
                        if (t1 > t_min && t1 < t_max && t1 < closest_t)
                        {
                            closest_t = t1;
                            closest_figure = elem;
                            Point3D point = camera + closest_t * D;
                            normal = point - closest_figure.Center;
                        }
                        if (t2 > t_min && t2 < t_max && t2 < closest_t)
                        {
                            closest_t = t2;
                            closest_figure = elem;
                            Point3D point = camera + closest_t * D;
                            normal = point - closest_figure.Center;
                        }
                        break;
                    case ElemType.Cube:
                    case ElemType.Plain:
                        var t_p = IntersectRayPlane(camera, D, elem);
                        var t1_p = t_p.Item1;
                        if (t1_p < closest_t && t_min < t1_p && t1_p < t_max)
                        {
                            closest_t = t1_p;
                            closest_figure = elem;
                            normal = t_p.Item2;
                        }
                        break;
                    default:
                        break;
                }
            }
            return (closest_figure, closest_t, normal);
        }

        /// <summary>
        /// Находит параметр t для нахождения точки пересечения со сферой (решает квадратное уравнение)
        /// </summary>
        public static (double, double) IntersectRaySphere(Point3D viewPoint, Point3D direction, Sphere sphere)
        {
            Point3D center = sphere.Center;
            double radius = sphere.Radius;
            Point3D OC = viewPoint - center;

            double k1 = ScalarProduct(direction, direction); // коэффициент a при квадратном уравнении
            double k2 =/* 2 * */ScalarProduct(OC, direction); // коэффициент b при квадратном уравнении
            double k3 = ScalarProduct(OC, OC) - radius * radius; // коэффициент c при квадратном уравнении

            double discriminant = k2 * k2 - /*4 * */k1 * k3;

            if (discriminant < 0) // если нет пересечений
                return (double.MaxValue, double.MaxValue);

            double t1 = (-k2 + Math.Sqrt(discriminant)) / (/*2 **/ k1);
            double t2 = (-k2 - Math.Sqrt(discriminant)) / (/*2 **/ k1);
            return (t1, t2);
        }

        /// <summary>
        /// Находит параметр t для нахождения точки пересечения с плоскостью
        /// </summary>
        public static (double, Point3D) IntersectRayPlane(Point3D camera, Point3D D, Figure figure)
        {
            double t = double.MaxValue;
            Point3D norm = new Point3D(0, 0, 0);
            foreach (Face face in figure.Faces)
            {
                var normal = Normalize(face.normal);
                var scalar = ScalarProduct(D, normal);
                if (scalar < eps)
                    continue;
                var d = ScalarProduct(face.center - camera, normal) / scalar;
                if (d < 0)
                    continue;
                var point = camera + d * D;
                if (d < t && face.Inside(point))
                {
                    t = d;
                    norm = -normal;
                }
            }
            return (t, norm);
        }
    }
}
