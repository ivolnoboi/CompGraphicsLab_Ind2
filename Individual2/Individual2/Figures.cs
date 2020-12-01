using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Individual2
{
    enum ElemType
    {
        Sphere,
        Cube, 
        Plane
    }
    class Figure
    {
        public ElemType type;
        public Color Color { get; set; }
        public int Specular { get; set; } // блики (или гладкость) -1 -- матовый 
        public float Reflective { get; set; } // отражение
    }
    class Sphere: Figure
    {
        //public ElemType type;
        public Point3D Center { get; set; }
        public float Radius { get; set; }

        public Sphere(Point3D center, float radius, Color color, int specular, float reflective)
        {
            Center = center;
            Radius = radius;
            Color = color;
            Specular = specular;
            Reflective = reflective;
            type = ElemType.Sphere;
        }
    }

    class Cube: Figure
    {

    }

    class Plane: Figure
    {
        public Point3D MinPoint;
        public Point3D MaxPoint;
        public Point3D Normal;
        //public ElemType type;

        public Plane(Point3D min, Point3D max, Point3D normal, Color color, int specular, float reflective)
        {
            MinPoint = min;
            MaxPoint = max;
            Normal = normal;
            Color = color;
            Specular = specular;
            Reflective = reflective;
            type = ElemType.Plane;
        }
    }
}
