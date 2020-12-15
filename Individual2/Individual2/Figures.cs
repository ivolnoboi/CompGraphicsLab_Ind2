using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Globalization;
using System.Windows.Forms;

namespace Individual2
{
    enum ElemType
    {
        Sphere,
        Cube,
        Plain
    }

    class Point2D
    {
        public double X;
        public double Y;
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    class Face
    {
        public List<Point3D> points;
        public Point3D center;
        public Point3D normal;
        bool xconst;
        bool yconst;
        bool zconst;

        public Face(List<Point3D> points)
        {
            this.points = points;
            center = findCenter();
            normal = new Point3D();
            xconst = points.All(point => point.X == points[0].X);
            yconst = points.All(point => point.Y == points[0].Y);
            zconst = points.All(point => point.Z == points[0].Z);
        }

        private bool PointBelongs(Point2D e1, Point2D e2, Point2D pt)
        {
            var a = e1.Y - e2.Y;
            var b = e2.X - e1.X;
            var c = e1.X * e2.Y - e2.X * e1.Y;
            if (Math.Abs(a * pt.X + b * pt.Y + c) > eps)
                return false;

            return lessEqual(Math.Min(e1.X, e2.X), pt.X)
                    && lessEqual(pt.X, Math.Max(e1.X, e2.X))
                    && lessEqual(Math.Min(e1.Y, e2.Y), pt.Y)
                    && lessEqual(pt.Y, Math.Max(e1.Y, e2.Y));
        }

        private bool IsCrossed(Point2D first1, Point2D first2, Point2D second1, Point2D second2)
        {
            var a1 = first1.Y - first2.Y;
            var b1 = first2.X - first1.X;
            var c1 = first1.X * first2.Y - first2.X * first1.Y;
            var a2 = second1.Y - second2.Y;
            var b2 = second2.X - second1.X;
            var c2 = second1.X * second2.Y - second2.X * second1.Y;
            var zn = a1 * b2 - a2 * b1;
            if (Math.Abs(zn) < eps)
                return false;

            var x = -1.0 * (c1 * b2 - c2 * b1) / zn;
            var y = -1.0 * (a1 * c2 - a2 * c1) / zn;
            if (equal(x, 0.0))
                x = 0.0;
            if (equal(y, 0.0))
                y = 0.0;
            var toFirst = lessEqual(Math.Min(first1.X, first2.X), x)
                    && lessEqual(x, Math.Max(first1.X, first2.X))
                    && lessEqual(Math.Min(first1.Y, first2.Y), y)
                    && lessEqual(y, Math.Max(first1.Y, first2.Y));
            var toSecond = lessEqual(Math.Min(second1.X, second2.X), x)
                    && lessEqual(x, Math.Max(second1.X, second2.X))
                    && lessEqual(Math.Min(second1.Y, second2.Y), y)
                    && lessEqual(y, Math.Max(second1.Y, second2.Y));
            return toFirst && toSecond;
        }

        public bool Inside(Point3D p)
        {
            var count = 0;
            if (zconst)
            {
                var pt = new Point2D(p.X, p.Y);
                var ray = new Point2D(100000.0, pt.Y);
                for (int i = 1; i <= points.Count; i++)
                {
                    var tmp1 = new Point2D(points[i - 1].X, points[i - 1].Y);
                    var tmp2 = new Point2D(points[i % points.Count].X, points[i % points.Count].Y);
                    if (PointBelongs(tmp1, tmp2, pt))
                        return true;
                    if (equal(tmp1.Y, tmp2.Y))
                        continue;
                    if (equal(pt.Y, Math.Min(tmp1.Y, tmp2.Y)))
                        continue;
                    if (equal(pt.Y, Math.Max(tmp1.Y, tmp2.Y)) && pt.X < Math.Min(tmp1.X, tmp2.X))
                        count++;
                    else if (IsCrossed(tmp1, tmp2, pt, ray))
                        count++;
                }
                return count % 2 != 0;
            }
            else if (yconst)
            {
                var pt = new Point2D(p.X, p.Z);
                var ray = new Point2D(100000.0, pt.Y);
                for (int i = 1; i <= points.Count; i++)
                {
                    var tmp1 = new Point2D(points[i - 1].X, points[i - 1].Z);
                    var tmp2 = new Point2D(points[i % points.Count].X, points[i % points.Count].Z);
                    if (PointBelongs(tmp1, tmp2, pt))
                        return true;
                    if (equal(tmp1.Y, tmp2.Y))
                        continue;
                    if (equal(pt.Y, Math.Min(tmp1.Y, tmp2.Y)))
                        continue;
                    if (equal(pt.Y, Math.Max(tmp1.Y, tmp2.Y)) && pt.X < Math.Min(tmp1.X, tmp2.X))
                        count++;
                    else if (IsCrossed(tmp1, tmp2, pt, ray))
                        count++;
                }
                return count % 2 != 0;
            }
            else if (xconst)
            {
                var pt = new Point2D(p.Y, p.Z);
                var ray = new Point2D(100000.0, pt.Y);
                for (int i = 1; i <= points.Count; i++)
                {
                    var tmp1 = new Point2D(points[i - 1].Y, points[i - 1].Z);
                    var tmp2 = new Point2D(points[i % points.Count].Y, points[i % points.Count].Z);
                    if (PointBelongs(tmp1, tmp2, pt))
                        return true;
                    if (equal(tmp1.Y, tmp2.Y))
                        continue;
                    if (equal(pt.Y, Math.Min(tmp1.Y, tmp2.Y)))
                        continue;
                    if (equal(pt.Y, Math.Max(tmp1.Y, tmp2.Y)) && pt.X < Math.Min(tmp1.X, tmp2.X))
                        count++;
                    else if (IsCrossed(tmp1, tmp2, pt, ray))
                        count++;
                }
                return count % 2 != 0;
            }
            return false;
        }

        // Векторное произведение векторов
        private static Point3D CrossProduct(Point3D vec1, Point3D vec2)
        {
            double x = vec1.Y * vec2.Z - vec1.Z * vec2.Y;
            double y = vec1.Z * vec2.X - vec1.X * vec2.Z;
            double z = vec1.X * vec2.Y - vec1.Y * vec2.X;
            return new Point3D(x, y, z);
        }

        public void findNormal(Point3D centerFigure)
        {
            Point3D Q = points[1];
            Point3D R = points[2];
            Point3D S = points[0];
            Point3D QR = R - Q;
            Point3D QS = S - Q;
            Point3D result = CrossProduct(QR, QS);

            Point3D CQ = Q - centerFigure;

            if (MultMatrix(result.ToMatrixRow(), CQ.ToMatrixCol())[0, 0] > eps)
                result = -result;
            normal = result;
        }

        public void translate(double x, double y, double z)
        {
            foreach (var point in points)
            {
                double[,] translateMatrix = { { 1.0, 0.0, 0.0, 0.0 },
                                              { 0.0, 1.0, 0.0, 0.0 },
                                              { 0.0, 0.0, 1.0, 0.0 },
                                              {   x,   y,   z, 1.0 } };

                double[,] xyz = { { point.X, point.Y, point.Z, 1.0 } };
                double[,] newPoint = MultMatrix(xyz, translateMatrix);
                point.X = newPoint[0, 0];
                point.Y = newPoint[0, 1];
                point.Z = newPoint[0, 2];
            }
            center = findCenter();
        }

        //перемножение матриц
        static public double[,] MultMatrix(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }

            return res;
        }

        private Point3D findCenter()
        {
            double x = points.Average(point => point.X);
            double y = points.Average(point => point.Y);
            double z = points.Average(point => point.Z);
            return new Point3D(x, y, z);
        }
        double eps = 0.000001;
        private bool equal(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < eps;
        }

        private bool lessEqual(double d1, double d2)
        {
            return d1 < d2 || equal(d1, d2);
        }
    }

    class Figure
    {
        public ElemType type;
        public Point3D Center;
        public Color Color = Color.White;
        public Material Material = new Material();
        public List<Face> Faces = new List<Face>();

        public virtual void translate(double x, double y, double z) { }
    }

    class Sphere : Figure
    {
        public double Radius = 0.0;
        public Sphere(Point3D center, double radius, Color color, Material material)
        {
            type = ElemType.Sphere;
            Radius = radius;
            Center = center;
            Color = color;
            Material = material;
        }

        public override void translate(double x, double y, double z)
        {
            Center += new Point3D(x, y, z);
        }
    }

    class Cube : Figure
    {
        public Cube(double size, Point3D bias, Color color, Material material)
        {
            type = ElemType.Cube;
            List<Point3D> allPoints = new List<Point3D>()
            {
                new Point3D(-size, size, size),
                new Point3D(size, size, size),
                new Point3D(size, -size, size),
                new Point3D(-size, -size, size),
                new Point3D(-size, size, -size),
                new Point3D(-size, -size, -size),
                new Point3D(size, -size, -size),
                new Point3D(size, size, -size)
            };

            var front = createCubeFace(allPoints[0], allPoints[1], allPoints[2], allPoints[3]);
            var back = createCubeFace(allPoints[4], allPoints[5], allPoints[6], allPoints[7]);
            var down = createCubeFace(allPoints[2], allPoints[6], allPoints[5], allPoints[3]);
            var top = createCubeFace(allPoints[4], allPoints[7], allPoints[1], allPoints[0]);
            var left = createCubeFace(allPoints[4], allPoints[0], allPoints[3], allPoints[5]);
            var right = createCubeFace(allPoints[7], allPoints[6], allPoints[2], allPoints[1]);

            Faces = new List<Face>() { front, back, down, top, left, right };
            Center = findCenter();
            translate(bias.X, bias.Y, bias.Z);
            Color = color;
            Material = material;
            findNormals();
        }

        private Face createCubeFace(Point3D p1, Point3D p2, Point3D p3, Point3D p4)
        {
            return new Face(new List<Point3D>() { new Point3D(p1), new Point3D(p2), new Point3D(p3), new Point3D(p4) });
        }

        public void findNormals()
        {
            foreach (var face in Faces)
                face.findNormal(Center);
        }

        private Point3D findCenter()
        {
            var x = Faces.Average(face => face.center.X);
            var y = Faces.Average(face => face.center.Y);
            var z = Faces.Average(face => face.center.Z);
            return new Point3D(x, y, z);
        }

        public override void translate(double x, double y, double z)
        {
            foreach (var face in Faces)
                face.translate(x, y, z);
            Center = findCenter();
        }
    }

    class Plain : Figure
    {
        public Plain(List<Face> faces, Point3D normal, Color color, Material material)
        {
            Faces = faces;
            Faces[0].normal = normal;
            findCenter();
            type = ElemType.Plain;
            Color = color;
            Material = material;
        }

        private Point3D findCenter()
        {
            var x = Faces.Average(face => face.center.X);
            var y = Faces.Average(face => face.center.Y);
            var z = Faces.Average(face => face.center.Z);
            return new Point3D(x, y, z);
        }

        public override void translate(double x, double y, double z)
        {
            foreach (var face in Faces)
                face.translate(x, y, z);
            Center = findCenter();
        }
    }
}
