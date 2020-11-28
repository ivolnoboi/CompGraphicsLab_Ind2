using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Individual2
{
    class Sphere
    {
        public Point3D Center { get; set; }
        public float Radius { get; set; }
        public Color Color { get; set; }
        public int Specular { get; set; }

        public Sphere(Point3D center, float radius, Color color, int specular)
        {
            Center = center;
            Radius = radius;
            Color = color;
            Specular = specular;
        }
    }
}
