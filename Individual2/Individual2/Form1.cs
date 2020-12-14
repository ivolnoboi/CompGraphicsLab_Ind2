﻿using System;
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
        public Form1()
        {
            InitializeComponent();
            light1.Checked = true;
            light2.Checked = true;
            matteCube.Checked = true;
            matteCube2.Checked = true;
            matteSphere.Checked = true;
            mirrorCube.Checked = true;
            mirrorSphere.Checked = true;
            transparentCube.Checked = true;
            transparentSphere.Checked = true;
        }
        private void addWall(List<Point3D> points, Point3D normal, Color color, Material material)
        {
            var f = new Face(points);
            var wall = new Figure(new List<Face>() { f });
            wall.Faces[0].normal = normal;
            wall.Color = color;
            wall.Material = material;
            scene.Add(wall);
        }

        private void addCube(double cube_half_size, Point3D point, Color color, Material material)
        {
            var cube = new Figure(new List<Face>());
            cube.createCube(cube_half_size);
            cube.translate(point.X, point.Y, point.Z);
            cube.Color = color;
            cube.Material = material;
            cube.findNormals();
            scene.Add(cube);
        }

        private void addSphere(Point3D point, double radius, Color color, Material material)
        {
            var sphere = new Figure(new List<Face>());
            sphere.createSphere(point, radius);
            sphere.Color = color;
            sphere.Material = material;
            scene.Add(sphere);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Material matte = new Material();
            Material mirror = new Material();
            if (enableMirror.Checked)
                mirror = new Material(1000, 1, 0);
            Material glass = new Material();
            if (enableTransparent.Checked)
                glass = new Material(0, 0, 1);
            if (matteSphere.Checked)
                addSphere(new Point3D(-5.0, 5.0, 1.0), 2.0, Color.LightCoral, matte);
            if (mirrorSphere.Checked)
                addSphere(new Point3D(0.0, 4.0, -3.0), 2.0, Color.White, mirror);
            if (transparentSphere.Checked)
                addSphere(new Point3D(4.5, 1.0, -5.0), 1.5, Color.White, glass);

            if (matteCube.Checked)
                addCube(1.0, new Point3D(3.0, 1.0, 0.0), Color.Red, matte);
            if (matteCube2.Checked)
                addCube(1.0, new Point3D(-4.5, 0.0, -6.0), Color.Green, matte);
            if (mirrorCube.Checked)
                addCube(1.0, new Point3D(1.7, 0, -5.0), Color.White, glass);
            if (transparentCube.Checked)
                addCube(1.0, new Point3D(-7.0, 1.0, 7.0), Color.White, mirror);

            var points = new List<Point3D>() {
            new Point3D(-10.0, -1.0, -10.0),
            new Point3D(-10.0, 10.0, -10.0),
            new Point3D(10.0, 10.0, -10.0),
            new Point3D(10.0, -1.0, -10.0)
        };
            if (back.Checked)
                addWall(points, new Point3D(0.0, 0.0, -1.0), Color.Yellow, mirror); // back
            else addWall(points, new Point3D(0.0, 0.0, -1.0), Color.Yellow, matte);
            points = new List<Point3D>() {
            new Point3D(-10.0, -1.0, 10.0),
            new Point3D(-10.0, 10.0, 10.0),
            new Point3D(10.0, 10.0, 10.0),
            new Point3D(10.0, -1.0, 10.0)
        };
            if (front.Checked)
                addWall(points, new Point3D(0.0, 0.0, 1.0), Color.Khaki, mirror); // front
            else addWall(points, new Point3D(0.0, 0.0, 1.0), Color.Khaki, matte);
            points = new List<Point3D>() {
            new Point3D(-10.0, -1.0, -10.0),
            new Point3D(-10.0, -1.0, 10.0),
            new Point3D(10.0, -1.0, 10.0),
            new Point3D(10.0, -1.0, -10.0)
        };
            addWall(points, new Point3D(0.0, -1.0, 0.0), Color.LightSteelBlue, matte); // пол
            points = new List<Point3D>() {
            new Point3D(-10.0, 10.0, -10.0),
            new Point3D(-10.0, 10.0, 10.0),
            new Point3D(10.0, 10.0, 10.0),
            new Point3D(10.0, 10.0, -10.0)
        };
            addWall(points, new Point3D(0.0, 1.0, 0.0), Color.LightGoldenrodYellow, matte); // потолок
            points = new List<Point3D>() {
            new Point3D(-10.0, -1.0, -10.0),
            new Point3D(-10.0, 10.0, -10.0),
            new Point3D(-10.0, 10.0, 10.0),
            new Point3D(-10.0, -1.0, 10.0)
        };
            if (left.Checked)
                addWall(points, new Point3D(-1.0, 0.0, 0.0), Color.BlueViolet, mirror); // left
            else addWall(points, new Point3D(-1.0, 0.0, 0.0), Color.BlueViolet, matte);
            points = new List<Point3D>() {
            new Point3D(10.0, -1.0, -10.0),
            new Point3D(10.0, 10.0, -10.0),
            new Point3D(10.0, 10.0, 10.0),
            new Point3D(10.0, -1.0, 10.0)
        };
            if (right.Checked)
                addWall(points, new Point3D(1.0, 0.0, 0.0), Color.Pink, mirror); // right
            else addWall(points, new Point3D(1.0, 0.0, 0.0), Color.Pink, matte);

            if (light1.Checked)
                lights.Add(new Light(LightType.Point, 0.6, new Point3D(0.0, 9.0, 0.0)));
            if (light2.Checked)
                lights.Add(new Light(LightType.Point, 0.8, new Point3D(-9.0, 5.0, -9.0)));

            pictureBox1.Image = RayTracing.CreateColorScene(pictureBox1.Width, pictureBox1.Height, scene, lights);

            lights.Clear();
            scene.Clear();
        }
    }
}
