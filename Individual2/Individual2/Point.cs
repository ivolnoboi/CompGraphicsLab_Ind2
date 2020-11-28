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
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Z { get; set; } = 0;

        public Point3D() { }
        public Point3D(float x, float y, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }
        static public Point3D operator -(Point3D point1, Point3D point2)
        {
            return new Point3D(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
        }
        static public Point3D operator *(float k, Point3D p)
        {
            return new Point3D(p.X * k, p.Y * k, p.Z * k);
        }

        static public Point3D operator *(Point3D p, float k)
        {
            return new Point3D(p.X * k, p.Y * k, p.Z * k);
        }

        static public Point3D operator +(Point3D point1, Point3D point2)
        {
            return new Point3D(point1.X + point2.X, point1.Y + point2.Y, point1.Z + point2.Z);
        }

        static public Point3D operator /(Point3D point, float k)
        {
            return new Point3D(point.X / k, point.Y / k, point.Z / k);
        }
    }
}
