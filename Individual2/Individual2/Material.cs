using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual2
{
    class Material
    {
        public int Specular { get; set; } = 0; // блики
        public double Reflective { get; set; } = 0.0; // отражение
        public double Transparent { get; set; } = 0.0; // прозрачность

        public Material() { }
        public Material(int specular, double reflective, double transparent)
        {
            Specular = specular;
            Reflective = reflective;
            Transparent = transparent;
        }
    }
}
