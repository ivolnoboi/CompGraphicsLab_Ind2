using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual2
{
    // по факту определяет положение смотрящего (положение обзора)
    class Camera
    {
        public Point3D Position;

        public Camera()
        {
            Position = new Point3D(0, 0, 0);
        }

        public Camera(float x, float y, float z)
        {
            Position = new Point3D(x, y, z);
        }
    }
}
