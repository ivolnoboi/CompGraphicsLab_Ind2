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
        List<Figure> scene = new List<Figure>();
        List<Light> lights = new List<Light>();
        Point3D positionLight2 = new Point3D(9.0, 5.0, -9.0);
        double min = 1.0;
        double max = 10.0;
        public Form1()
        {
            InitializeComponent();
            light1.Checked = true;
            light2.Checked = true;
            matteCube.Checked = true;
            matteSphere2.Checked = true;
            matteSphere.Checked = true;
            mirrorCube.Checked = true;
            mirrorSphere.Checked = true;
            transparentCube.Checked = true;
            transparentSphere.Checked = true;
            enableMirror.Checked = true;
            enableTransparent.Checked = true;
        }

        private void addWalls(Material mirror, Material matte)
        {
            Plain wall;

            // ЗАДНЯЯ СТЕНА
            Face face = new Face(new List<Point3D>(){new Point3D(-max, -min, -max), new Point3D(-max, max, -max),
                                 new Point3D(max, max, -max), new Point3D(max, -min, -max) });
            if (back.Checked)
                wall = new Plain(new List<Face>() { face }, new Point3D(0.0, 0.0, -1.0), Color.Yellow, mirror);
            else wall = new Plain(new List<Face>() { face }, new Point3D(0.0, 0.0, -1.0), Color.Yellow, matte);
            scene.Add(wall);

            // ПЕРЕДНЯЯ СТЕНА
            face = new Face(new List<Point3D>() {new Point3D(-max, -min, max), new Point3D(-max, max, max),
                                                 new Point3D(max, max, max), new Point3D(max, -min, max)});
            if (front.Checked)
                wall = new Plain(new List<Face>() { face }, new Point3D(0.0, 0.0, 1.0), Color.Khaki, mirror);
            else wall = new Plain(new List<Face>() { face }, new Point3D(0.0, 0.0, 1.0), Color.Khaki, matte);
            scene.Add(wall);

            // ПОЛ
            face = new Face(new List<Point3D>() {new Point3D(-max, -min, -max), new Point3D(-max, -min, max),
                                                 new Point3D(max, -min, max), new Point3D(max, -min, -max)});
            wall = new Plain(new List<Face>() { face }, new Point3D(0.0, -1.0, 0.0), Color.LightSteelBlue, matte);
            scene.Add(wall);

            // ПОТОЛОК
            face = new Face(new List<Point3D>() {new Point3D(-max, max, -max), new Point3D(-max, max, max),
                                                 new Point3D(max, max, max), new Point3D(max, max, -max)});
            wall = new Plain(new List<Face>() { face }, new Point3D(0.0, 1.0, 0.0), Color.LightGoldenrodYellow, matte);
            scene.Add(wall);

            // ЛЕВАЯ СТЕНА
            face = new Face(new List<Point3D>() {new Point3D(-max, -min, -max), new Point3D(-max, max, -max),
                                                 new Point3D(-max, max, max), new Point3D(-max, -min, max)});
            if (left.Checked)
                wall = new Plain(new List<Face>() { face }, new Point3D(-1.0, 0.0, 0.0), Color.BlueViolet, mirror);
            else wall = new Plain(new List<Face>() { face }, new Point3D(-1.0, 0.0, 0.0), Color.BlueViolet, matte);
            scene.Add(wall);

            // ПРАВАЯ СТЕНА
            face = new Face(new List<Point3D>() {new Point3D(max, -min, -max), new Point3D(max, max, -max),
                                                 new Point3D(max, max, max), new Point3D(max, -min, max)});
            if (right.Checked)
                wall = new Plain(new List<Face>() { face }, new Point3D(1.0, 0.0, 0.0), Color.Pink, mirror);
            else wall = new Plain(new List<Face>() { face }, new Point3D(1.0, 0.0, 0.0), Color.Pink, matte);
            scene.Add(wall);
        }

        private void addSpheres(Material matte, Material mirror, Material glass)
        {
            Sphere sphere;
            if (matteSphere.Checked)
            {
                sphere = new Sphere(new Point3D(-8.0, 5.0, 4.0), 1.0, Color.LightCoral, matte);
                scene.Add(sphere);
            }
            if (mirrorSphere.Checked)
            {
                sphere = new Sphere(new Point3D(5.0, 5.0, 3.0), 2.0, Color.White, mirror);
                scene.Add(sphere);
            }
            if (transparentSphere.Checked)
            {
                sphere = new Sphere(new Point3D(0.0, 4.0, -5.0), 1.5, Color.White, glass);
                scene.Add(sphere);
            }
            if (matteSphere2.Checked)
            {
                sphere = new Sphere(new Point3D(-4.5, 0.0, -6.0), 1.0, Color.Green, matte);
                scene.Add(sphere);
            }
        }

        private void addCubes(Material matte, Material mirror, Material glass)
        {
            Cube cube;
            if (matteCube.Checked)
            {
                cube = new Cube(1.0, new Point3D(4.0, 0.5, 0.0), Color.Red, matte);
                scene.Add(cube);
            }
            if (mirrorCube.Checked)
            {
                cube = new Cube(1.0, new Point3D(-4.0, 1.5, 1.0), Color.White, mirror);
                scene.Add(cube);
            }
            if (transparentCube.Checked)
            {
                cube = new Cube(1.0, new Point3D(2.0, 0, -5.0), Color.White, glass);
                scene.Add(cube);
            }
        }

        private void addLights()
        {
            if (light1.Checked)
                lights.Add(new Light(0.6, new Point3D(0.0, 9.0, 0.0)));
            if (light2.Checked)
                lights.Add(new Light(0.8, positionLight2));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Material matte = new Material();
            Material mirror = new Material();
            if (enableMirror.Checked)
                mirror = new Material(0, 1, 0);
            Material glass = new Material();
            if (enableTransparent.Checked)
                glass = new Material(0, 0, 1);

            addSpheres(matte, mirror, glass);
            addCubes(matte, mirror, glass);
            addWalls(mirror, matte);
            addLights();

            pictureBox1.Image = RayTracing.CreateColorScene(pictureBox1.Width, pictureBox1.Height, scene, lights);

            lights.Clear();
            scene.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            positionLight2 = new Point3D(-9.0, 5.0, 9.0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            positionLight2 = new Point3D(9.0, 5.0, 9.0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            positionLight2 = new Point3D(-9.0, 5.0, -9.0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            positionLight2 = new Point3D(9.0, 5.0, -9.0);
        }
    }
}
