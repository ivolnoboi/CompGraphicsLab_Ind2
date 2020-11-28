using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual2
{
    enum Type
    {
        Ambient, // окружающее освещение
        Point, // точечный
        Directional // направленный
    }

    class Light
    {
        public Type Type;
        public float Intensity;
        public Point3D Position;
        public Point3D Direction;

        public Light(Type type, float intensity)
        {
            Type = type;
            Intensity = intensity;
        }

        public Light(Type type, float intensity, Point3D otherParam)
        {
            Type = type;
            Intensity = intensity;
            if (type==Type.Point)
            {
                Position = otherParam;
            }
            else
            {
                Direction = otherParam;
            }
        }
    }
}
