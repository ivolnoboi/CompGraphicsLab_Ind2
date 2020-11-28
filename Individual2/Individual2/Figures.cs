using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Individual2
{
    class Figure
    {

    }
    class Sphere: Figure
    {
        public Point3D Center { get; set; }
        public float Radius { get; set; }
        public Color Color { get; set; }
        public int Specular { get; set; } // блики (или гладкость) -1 -- матовый 
        public float Reflective { get; set; } // отражение

        public Sphere(Point3D center, float radius, Color color, int specular, float reflective)
        {
            Center = center;
            Radius = radius;
            Color = color;
            Specular = specular;
            Reflective = reflective;
        }
    }

    class Cube: Figure
    {

    }

    class Wall: Figure
    {

    }
}
