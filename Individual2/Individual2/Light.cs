using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual2
{
    class Light
    {
        public double Intensity;
        public Point3D Position;

        public Light(double intensity, Point3D position)
        {
            Intensity = intensity;
            Position = position;
        }
    }
}