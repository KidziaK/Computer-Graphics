using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CG_05_3D
{
    public partial class Form1 : Form
    {
        int Sx, Sy;
        float Cx, Cy, d;


        public class Vector
        {
            public int x;
            public int y;
            public int z;

            public Vector(int _x, int _y, int _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }
        }
        public class Edge
        {
            public Vector A;
            public Vector B;

            public Edge(Vector _a, Vector _b)
            {
                A = new Vector(_a.x, _a.y, _a.z);
                B = new Vector(_b.x, _b.y, _b.z);
            }

        }

        public List<Edge> pyramid = new List<Edge>();
        public void MakePyramid()
        {
            pyramid.Add(new Edge(new Vector(-5 + (int)Cx, -10 + (int)Cy, 25), new Vector(5, -10, 25)));

            pyramid.Add(new Edge(new Vector(-5 + (int)Cx, -10 + (int)Cy, 25), new Vector(-5, -10, 35)));

            pyramid.Add(new Edge(new Vector(-5 + (int)Cx, -10 + (int)Cy, 25), new Vector(0, -10, 30)));

            pyramid.Add(new Edge(new Vector(5 + (int)Cx, -10 + (int)Cy, 35), new Vector(5, -10, 25)));

            pyramid.Add(new Edge(new Vector(5 + (int)Cx, -10 + (int)Cy, 35), new Vector(0, 10, 30)));

            pyramid.Add(new Edge(new Vector(5 + (int)Cx, -10 + (int)Cy, 35), new Vector(-5, -10, 35)));

            pyramid.Add(new Edge(new Vector(5 + (int)Cx, -10 + (int)Cy, 25), new Vector(0, 10, 30)));

            pyramid.Add(new Edge(new Vector(-5 + (int)Cx, -10 + (int)Cy, 35), new Vector(0, 10, 30)));
        }

        public void Project(ref Edge e)
        {

        }
        public void ProjectPyramid()
        {
            foreach(Edge e in pyramid)
            {
                e.A.x = (int)((float)e.A.x * d + (float)e.A.z * Cx);
                e.A.y = (int)((float)e.A.y * (-d) + (float)e.A.z * Cy);
                e.A.z = 0;

                e.B.x = (int)((float)e.B.x * d + (float)e.B.z * Cx);
                e.B.y = (int)((float)e.B.y * (-d) + (float)e.B.z * Cy);
                e.B.z = 0;
            }
        }
        public Form1()
        {
            



            InitializeComponent();

            pictureBoxImage.Image = new Bitmap(pictureBoxImage.Width, pictureBoxImage.Height);

            

            Sx = pictureBoxImage.Width / 2;
            Sy = pictureBoxImage.Height / 2;
            //Cx = (float)Sx / (float)2;
            //Cy = (float)Sy / (float)2;
            //d = ((float)Sx / (float)2);
            Cx = 2;
            Cy = 2;
            d = 5;

            MakePyramid();
            ProjectPyramid();

            foreach (Edge e in pyramid)
            {
                Graphics g = Graphics.FromImage(pictureBoxImage.Image);
                g.DrawLine(Pens.Black, new Point(e.A.x, e.A.y), new Point(e.B.x, e.B.y));

                g.Dispose();
               
            }

            pictureBoxImage.Refresh();
        }
    }
}
