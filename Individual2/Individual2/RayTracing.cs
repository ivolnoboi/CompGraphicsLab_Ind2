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
        private static Color backgroundColor = Color.LightCoral;
        private static double eps = 0.0001f;
        private static List<Figure> Scene;
        private static List<Light> Lights;

        // Скалярное произведение
        public static double ScalarProduct(Point3D vector1, Point3D vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        // Длина вектора
        public static double Lenght(Point3D vec)
        {
            return Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
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
            int red = (int)(color.R * value);
            int green = (int)(color.G * value);
            int blue = (int)(color.B * value);
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

        // Заполняет буфер цвета
        public static Bitmap CreateColorScene(int width, int height, List<Figure> scene, List<Light> lights)
        {
            Scene = scene;
            Lights = lights;
            Point3D camera = new Point3D(0.0, 4.0, -20.0);

            Color[,] colors1 = new Color[height + 1, width + 1];
            for (int y = (-height / 2) + 1; y < height / 2; y++)
                Parallel.For((-width / 2) + 1, width / 2, x =>
               {
                   //for (int x = (-width / 2) + 1; x < width / 2; x++)
                   // {
                   Point3D D = CanvasToViewport(x, y, width, height);
                   Color color = TraceRay(camera, D, 1.0, double.MaxValue, 1);
                   int imgX = x + width / 2;
                   int imgY = height / 2 - y;
                   if (!(imgX < 0 || imgX >= width || imgY < 0 || imgY >= height))
                       // continue;
                       colors1[y + height / 2, x + height / 2] = color;
                   // }
               });

            Bitmap bmp = new Bitmap(width, height);
            for (int y = (-height / 2) + 1; y < height / 2; y++)
                for (int x = (-width / 2) + 1; x < width / 2; x++)
                {
                    int imgX = x + width / 2;
                    int imgY = height / 2 - y;
                    bmp.SetPixel(imgX, imgY, colors1[y + height / 2, x + height / 2]);
                }
            return bmp;
        }


        // Преобразовать координаты холста в координаты окна просмотра (плоскости проекции)
        public static Point3D CanvasToViewport(int x, int y, int width, int height)
        {
            double viewWindowWidth = 1;
            double viewWindowHeight = 1;
            double ditanceFromCamera = 1.0;
            double X = x * viewWindowWidth / width;
            double Y = y * viewWindowHeight / height;
            return new Point3D(X, Y, ditanceFromCamera);
        }
        /*
        // Принадлежит ли точка данной грани
        private static bool pointIsInPlane(Plane plain, Point3D point)
        {
            return (point.X - Math.Max(plain.MaxPoint.X, plain.MinPoint.X) <= eps) && (point.X - Math.Min(plain.MaxPoint.X, plain.MinPoint.X) >= -eps)
                && (point.Y - Math.Max(plain.MaxPoint.Y, plain.MinPoint.Y) <= eps) && (point.Y - Math.Min(plain.MaxPoint.Y, plain.MinPoint.Y) >= -eps)
                && (point.Z - Math.Max(plain.MaxPoint.Z, plain.MinPoint.Z) <= eps) && (point.Z - Math.Min(plain.MaxPoint.Z, plain.MinPoint.Z) >= -eps);
        }*/

        private static double computeLighting(Point3D point, Point3D normal, Point3D view, double specular)
        {
            double intensity = 0;
            double lengthN = Lenght(normal);
            double lengthV = Lenght(view);
            double t_max;
            foreach (var light in Lights)
            {
                if (light.Type == LightType.Ambient)
                {
                    intensity += light.Intensity;
                }
                else
                {
                    Point3D vectorLight;
                    if (light.Type == LightType.Point)
                    {
                        vectorLight = light.Position - point;
                        t_max = 1.0;
                    }
                    else
                    {
                        vectorLight = light.Position;
                        t_max = double.MaxValue;
                    }
                    var (blocker, _, _) = closestIntersection(point, vectorLight, eps, t_max);
                    double tr = 1.0;
                    if (blocker != null)
                        continue;
                    double nDotL = ScalarProduct(normal, vectorLight);
                    if (nDotL > 0)
                    {
                        intensity += tr * light.Intensity * nDotL / (lengthN * Lenght(vectorLight));
                    }
                    if (specular > 0)
                    {
                        Point3D vecR = ReflectRay(vectorLight, normal);
                        double rDotV = ScalarProduct(vecR, view);
                        if (rDotV > 0)
                        {
                            intensity += tr * light.Intensity * Math.Pow(rDotV / (Lenght(vecR) * lengthV), specular);
                        }
                    }
                }
            }
            return intensity;
        }

        public static Color ReflectiveColor(Color local, Color reflective, double coefficient)
        {
            Color color1 = colorProd(local, 1 - coefficient);
            Color color2 = colorProd(reflective, coefficient);
            return colorSum(color1, color2);
        }

        private static Color increase(double k, Color c)
        {
            var a = c.A;
            var r = GetCorrectValue((int)(c.R * k + 5));
            var g = GetCorrectValue((int)(c.G * k + 5));
            var b = GetCorrectValue((int)(c.B * k + 5));
            return Color.FromArgb(a, r, g, b);
        }

        private static Color TraceRay(Point3D camera, Point3D D, double t_min, double t_max, double depth, int recurseDepth = 5)
        {
            var (closest_elem, closest_t, normal) = closestIntersection(camera, D, t_min, t_max);
            if (closest_elem == null)
                return backgroundColor;
            normal = Normalize(normal);

            Point3D point = camera + closest_t * D;

            double lightK = computeLighting(point, normal, -D, closest_elem.Material.Specular);
            Color localColor = increase(lightK, closest_elem.Color);
            if (recurseDepth <= 0 || depth <= eps)
                return localColor;

            Point3D reflectedRay = ReflectRay(-D, normal);
            Color reflectionColor = TraceRay(point, reflectedRay, eps, double.MaxValue, depth * closest_elem.Material.Reflective, recurseDepth - 1);
            Color reflected = colorSum(increase(1 - closest_elem.Material.Reflective, localColor), increase(closest_elem.Material.Reflective, reflectionColor));
            if (closest_elem.Material.Transparent <= 0)
                return increase(depth, reflected);

            Point3D refracted = RefractRay(D, normal, 1.5);
            Color trColor = TraceRay(point, refracted, eps, double.MaxValue, depth * closest_elem.Material.Transparent, recurseDepth - 1);
            Color transparent = colorSum(increase(1 - closest_elem.Material.Transparent, reflected), increase(closest_elem.Material.Transparent, trColor));
            return increase(depth, transparent);
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
        private static (Figure, double, Point3D) closestIntersection(Point3D camera, Point3D D, double t_min, double t_max)
        {
            Figure closest_figure = null;
            double closest_t = double.MaxValue;
            Point3D norm = new Point3D(0, 0, 0);
            foreach (var elem in Scene)
            {
                if (elem.type == ElemType.Sphere)
                {
                    var t = IntersectRaySphere(camera, D, elem);
                    double t1 = t.Item1, t2 = t.Item2;
                    if (t1 > t_min && t1 < t_max && t1 < closest_t)
                    {
                        closest_t = t1;
                        closest_figure = elem;
                    }
                    if (t2 > t_min && t2 < t_max && t2 < closest_t)
                    {
                        closest_t = t2;
                        closest_figure = elem;
                    }
                }
                else
                {
                    var t = IntersectRayPlane(camera, D, elem);
                    var t1 = t.Item1;
                    if (t1 < closest_t && t_min < t1 && t1 < t_max)
                    {
                        closest_t = t1;
                        closest_figure = elem;
                        norm = t.Item2;
                    }
                }
            }
            if (closest_figure != null && closest_figure.type == ElemType.Sphere)
            {
                Point3D point = camera + closest_t * D;
                norm = point - closest_figure.Center;
            }
            return (closest_figure, closest_t, norm);
        }

        /// <summary>
        /// Находит параметр t для нахождения точки пересечения со сферой (решает квадратное уравнение)
        /// </summary>
        public static (double, double) IntersectRaySphere(Point3D viewPoint, Point3D direction, Figure sphere)
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
        public static (double, Point3D) IntersectRayPlane(Point3D camera, Point3D D, Figure polyhedron)
        {
            double t = double.MaxValue;
            Point3D norm = new Point3D(0, 0, 0);
            foreach (var face in polyhedron.Faces)
            {
                var normal = Normalize(face.normal);
                var scalar = ScalarProduct(D, normal);
                if (scalar < eps)
                    continue;
                var d = ScalarProduct(face.center - camera, normal) / scalar;
                if (d < 0)
                    continue;
                var point = camera + d * D;
                if (t > d && face.inside(point))
                {
                    t = d;
                    norm = -normal;
                }
            }
            return (t, norm);
        }
    }
}
