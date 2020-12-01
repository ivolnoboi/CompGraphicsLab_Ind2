using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Individual2
{
    public partial class Form1 : Form
    {
        Bitmap image;
        public Form1()
        {
            InitializeComponent();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Figure> scene = new List<Figure>();
            List<Light> lights = new List<Light>();
            //scene.Add(new Sphere(new Point3D(0, -1, 3), 1, Color.Red, 500, 0.2f));
            //scene.Add(new Sphere(new Point3D(2, 0, 4), 1, Color.Blue, 500, 0.3f));
            //scene.Add(new Sphere(new Point3D(-2, 0, 4), 1, Color.Green, 10, 0.4f));
            //scene.Add(new Sphere(new Point3D(0, -5001, 0), 5000, Color.Yellow, 1000, 0f));
            //scene.Add(new Plane(new Point3D(0, 0, 50), new Point3D(10, 10, 50), new Point3D(0, 0, 1), Color.Green, 10, 0.4f));
            scene.Add(new Plane(new Point3D(-25, -25, 70), new Point3D(25, 25, 70), new Point3D(0, 0, 1), Color.White, 10, 0.5f)); // дальняя стена
            scene.Add(new Plane(new Point3D(-25, -25, -70), new Point3D(25, -25, 70), new Point3D(0, -1, 0), Color.AliceBlue, 10, 0.0f)); // пол
            scene.Add(new Plane(new Point3D(-25, 25, -70), new Point3D(25, 25, 70), new Point3D(0, 1, 0), Color.Blue, 10, 0.0f)); // потолок
            scene.Add(new Plane(new Point3D(-25, -25, -70), new Point3D(-25, 25, 70), new Point3D(-1, 0, 0), Color.Green, 10, 0.0f)); // стена слева
            scene.Add(new Plane(new Point3D(25, -25, -70), new Point3D(25, 25, 70), new Point3D(1, 0, 0), Color.Red, 10, 0.0f)); // стена справа
            scene.Add(new Plane(new Point3D(-25, -25, -70), new Point3D(25, 25, -70), new Point3D(0, 0, -1), Color.Red, 10, 0.9f)); // стена за камерой

            //scene.Add(new Sphere(new Point3D(0, 0, 50), 10, Color.Blue, 1000, 0.9f));
            //scene.Add(new Sphere(new Point3D(20, -10, 60), 5, Color.Green, 10, 0.4f));

            lights.Add(new Light(Type.Ambient, 0.2f));
            lights.Add(new Light(Type.Point, 0.8f, new Point3D(12, 24, 10)));
            lights.Add(new Light(Type.Point, 0.6f, new Point3D(12, 24, 20)));
            lights.Add(new Light(Type.Directional, 0.2f, new Point3D(1, 4, 4)));

            pictureBox1.Image = RayTracing.CreateColorScene(pictureBox1.Width, pictureBox1.Height, scene, lights);
        }
    }
}
