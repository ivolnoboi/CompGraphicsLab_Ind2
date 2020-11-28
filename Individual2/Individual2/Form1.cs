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
            List<Sphere> scene = new List<Sphere>();
            List<Light> lights = new List<Light>();
            scene.Add(new Sphere(new Point3D(0, -1, 3), 1, Color.Red, 500, 0.2f));
            scene.Add(new Sphere(new Point3D(2, 0, 4), 1, Color.Blue, 500, 0.3f));
            scene.Add(new Sphere(new Point3D(-2, 0, 4), 1, Color.Green, 10, 0.4f));
            scene.Add(new Sphere(new Point3D(0, -5001, 0), 5000, Color.Yellow, 1000, 0f));
            lights.Add(new Light(Type.Ambient, 0.2f));
            lights.Add(new Light(Type.Point, 0.6f, new Point3D(2, 1, 0)));
            lights.Add(new Light(Type.Directional, 0.2f, new Point3D(1, 4, 4)));

            pictureBox1.Image = RayTracing.CreateColorScene(pictureBox1.Width, pictureBox1.Height, scene, lights);
        }
    }
}
