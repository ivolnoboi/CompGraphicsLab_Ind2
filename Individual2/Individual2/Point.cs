using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual2
{
    class Point3D
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
        public double Z { get; set; } = 0;

        public Point3D() { }
        public Point3D(double x, double y, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Point3D(Point3D point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }
        static public Point3D operator -(Point3D point1, Point3D point2)
        {
            return new Point3D(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
        }
        static public Point3D operator *(double k, Point3D p)
        {
            return new Point3D(k * p.X, k * p.Y, k * p.Z);
        }

        static public Point3D operator *(Point3D p, double k)
        {
            return new Point3D(p.X * k, p.Y * k, p.Z * k);
        }

        static public Point3D operator +(Point3D point1, Point3D point2)
        {
            return new Point3D(point1.X + point2.X, point1.Y + point2.Y, point1.Z + point2.Z);
        }

        static public Point3D operator /(Point3D point, double k)
        {
            return new Point3D(point.X / k, point.Y / k, point.Z / k);
        }

        static public Point3D operator -(Point3D point)
        {
            return new Point3D(-point.X, -point.Y, -point.Z);
        }

        public List<double> ToList()
        {
            return new List<double>() { X, Y, Z };
        }

        static public Point3D ToPoint(List<double> lst)
        {
            return new Point3D(lst[0], lst[1], lst[2]);
        }

        public double[,] ToMatrixRow()
        {
            return new double[,]{ { X, Y, Z} };
        }

        public double[,] ToMatrixCol()
        {
            return new double[,] { { X }, { Y }, { Z } };
        }
    }
}
