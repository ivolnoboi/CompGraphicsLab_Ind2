using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Globalization;

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
        bool xconst; // points.all { point -> point.x == points[0].x }
        bool yconst; // points.all { point -> point.x == points[0].y }
        bool zconst; // points.all { point -> point.x == points[0].z }

        public Face(List<Point3D> points)
        {
            this.points = points;
            center = findCenter();
            normal = new Point3D();
            xconst = points.All(point => point.X == points[0].X);
            yconst = points.All(point => point.Y == points[0].Y);
            zconst = points.All(point => point.Z == points[0].Z);
        }

        public Face(Face face)
        {
            points = face.points.ToList();
            center = new Point3D(face.center);
            normal = new Point3D(face.normal);
            xconst = face.xconst;
            yconst = face.yconst;
            zconst = face.zconst;
        }
        private bool pointBelongs(Point2D e1, Point2D e2, Point2D pt)
        {
            var a = e1.Y - e2.Y;
            var b = e2.X - e1.X;
            var c = e1.X * e2.Y - e2.X * e1.Y;
            if (Math.Abs(a * pt.X + b * pt.Y + c) > eps)
                return false;

            return lEq(Math.Min(e1.X, e2.X), pt.X)
                    && lEq(pt.X, Math.Max(e1.X, e2.X))
                    && lEq(Math.Min(e1.Y, e2.Y), pt.Y)
                    && lEq(pt.Y, Math.Max(e1.Y, e2.Y));
        }

        private bool isCrossed(Point2D first1, Point2D first2, Point2D second1, Point2D second2)
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
            var toFirst = lEq(Math.Min(first1.X, first2.X), x)
                    && lEq(x, Math.Max(first1.X, first2.X))
                    && lEq(Math.Min(first1.Y, first2.Y), y)
                    && lEq(y, Math.Max(first1.Y, first2.Y));
            var toSecond = lEq(Math.Min(second1.X, second2.X), x)
                    && lEq(x, Math.Max(second1.X, second2.X))
                    && lEq(Math.Min(second1.Y, second2.Y), y)
                    && lEq(y, Math.Max(second1.Y, second2.Y));
            return toFirst && toSecond;
        }

        public bool inside(Point3D p)
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
                    if (pointBelongs(tmp1, tmp2, pt))
                        return true;
                    if (equal(tmp1.Y, tmp2.Y))
                        continue;
                    if (equal(pt.Y, Math.Min(tmp1.Y, tmp2.Y)))
                        continue;
                    if (equal(pt.Y, Math.Max(tmp1.Y, tmp2.Y)) && less(pt.X, Math.Min(tmp1.X, tmp2.X)))
                        count++;
                    else if (isCrossed(tmp1, tmp2, pt, ray))
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
                    if (pointBelongs(tmp1, tmp2, pt))
                        return true;
                    if (equal(tmp1.Y, tmp2.Y))
                        continue;
                    if (equal(pt.Y, Math.Min(tmp1.Y, tmp2.Y)))
                        continue;
                    if (equal(pt.Y, Math.Max(tmp1.Y, tmp2.Y)) && less(pt.X, Math.Min(tmp1.X, tmp2.X)))
                        count++;
                    else if (isCrossed(tmp1, tmp2, pt, ray))
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
                    if (pointBelongs(tmp1, tmp2, pt))
                        return true;
                    if (equal(tmp1.Y, tmp2.Y))
                        continue;
                    if (equal(pt.Y, Math.Min(tmp1.Y, tmp2.Y)))
                        continue;
                    if (equal(pt.Y, Math.Max(tmp1.Y, tmp2.Y)) && less(pt.X, Math.Min(tmp1.X, tmp2.X)))
                        count++;
                    else if (isCrossed(tmp1, tmp2, pt, ray))
                        count++;
                }
                return count % 2 != 0;
            }
            return false;
        }

        public void findNormal(Point3D polyhedronCenter)
        {
            var Q = points[1];
            var R = points[2];
            var S = points[0];
            var QR = new List<double>() { R.X - Q.X, R.Y - Q.Y, R.Z - Q.Z };
            var QS = new List<double>() { S.X - Q.X, S.Y - Q.Y, S.Z - Q.Z };
            var result = new List<double>
            {
                QR[1] * QS[2] - QR[2] * QS[1],
                -(QR[0] * QS[2] - QR[2] * QS[0]),
                QR[0] * QS[1] - QR[1] * QS[0]
            };

            var CQ = new List<double> { Q.X - polyhedronCenter.X, Q.Y - polyhedronCenter.Y, Q.Z - polyhedronCenter.Z };
            if (mulMatrix(result, 1, 3, CQ, 3, 1)[0] > eps)
            {
                result[0] = result[0] * -1;
                result[1] = result[1] * -1;
                result[2] = result[2] * -1;
            }
            normal = Point3D.ToPoint(result);
        }

        public void translate(double x, double y, double z)
        {
            foreach (var point in points)
            {
                var t = new List<double>() {
                    1.0, 0.0, 0.0, 0.0,
                    0.0, 1.0, 0.0, 0.0,
                    0.0, 0.0, 1.0, 0.0,
                    x, y, z, 1.0
                };
                var xyz = new List<double>() { point.X, point.Y, point.Z, 1.0 };
                var c = mulMatrix(xyz, 1, 4, t, 4, 4);
                point.X = c[0];
                point.Y = c[1];
                point.Z = c[2];
            }
            center = findCenter();
        }
        private List<double> mulMatrix(List<double> matr1, int m1, int n1, List<double> matr2, int m2, int n2)
        {
            if (n1 != m2)
                return new List<double>();
            var c = new List<double>();
            for (int i = 0; i < m1 * n2; i++)
                c.Add(0);
            for (int i = 0; i < m1; i++)
                for (int j = 0; j < n2; j++)
                    for (int r = 0; r < n1; r++)
                        c[i * m1 + j] += matr1[i * m1 + r] * matr2[r * n2 + j];
            return c;
        }
        private Point3D findCenter()
        {
            double x = 0.0, y = 0.0, z = 0.0;
            foreach (var point in points)
            {
                x += point.X;
                y += point.Y;
                z += point.Z;
            }
            x /= points.Count;
            y /= points.Count;
            z /= points.Count;

            return new Point3D(x, y, z);
        }
        double eps = 0.000001;
        private bool equal(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < eps;
        }

        private bool less(double d1, double d2)
        {
            return d1 < d2 && Math.Abs(d1 - d2) >= eps;
        }

        private bool lEq(double d1, double d2)
        {
            return less(d1, d2) || equal(d1, d2);
        }
    }

    class Figure
    {
        public ElemType type;
        public Point3D Center;
        public double Radius = 0.0;
        public Color Color = Color.White;
        public Material Material = new Material();
        public List<Face> Faces;

        public Figure(List<Face> faces)
        {
            if (faces.Count != 0)
            {
                Faces = faces;
                findCenter();
                if (faces.Count == 1)
                    type = ElemType.Plain;
            }
        }

        public void createCube(double size = 50.0)
        {
            type = ElemType.Cube;
            var front = makeFace(
                new Point3D(-size, size, size),
                new Point3D(size, size, size),
                new Point3D(size, -size, size),
                new Point3D(-size, -size, size)
            );

            var back = makeFace(
                new Point3D(-size, size, -size),
                new Point3D(-size, -size, -size),
                new Point3D(size, -size, -size),
                new Point3D(size, size, -size)
            );

            var down = makeFace(front.points[2], back.points[2], back.points[1], front.points[3]);
            var top = makeFace(back.points[0], back.points[3], front.points[1], front.points[0]);
            var left = makeFace(back.points[0], front.points[0], front.points[3], back.points[1]);
            var right = makeFace(back.points[3], back.points[2], front.points[2], front.points[1]);

            Faces = new List<Face>() { front, back, down, top, left, right };
            Center = findCenter();
        }

        public void createSphere(Point3D center, double radius)
        {
            type = ElemType.Sphere;
            Radius = radius;
            Faces = new List<Face>() { new Face(new List<Point3D>() { new Point3D(center) }) };
            Center = center;
        }

        public void findNormals()
        {
            foreach (var face in Faces)
                face.findNormal(Center);
        }

        private Face makeFace(Point3D p1, Point3D p2, Point3D p3, Point3D p4)
        {
            return new Face(new List<Point3D>() { new Point3D(p1), new Point3D(p2), new Point3D(p3), new Point3D(p4) });
        }

        private Point3D findCenter()
        {
            var x = 0.0;
            var y = 0.0;
            var z = 0.0;
            foreach (var face in Faces)
            {
                x += face.center.X;
                y += face.center.Y;
                z += face.center.Z;
            }
            x /= Faces.Count;
            y /= Faces.Count;
            z /= Faces.Count;

            return new Point3D(x, y, z);
        }

        public void translate(double x, double y, double z)
        {
            foreach (var face in Faces)
                face.translate(x, y, z);
            Center = findCenter();
        }
    }
}
