using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual2
{
    enum LightType 
    { 
        Ambient, // окружающее освещение
        Point // точечный источник света
    }

    class Light
    {
        public LightType Type;
        public double Intensity;
        public Point3D Position;

        public Light(LightType type, double intensity, Point3D position)
        {
            Type = type;
            Intensity = intensity;
            Position = position;
        }
    }
}