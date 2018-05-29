using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace CG_04_ClippingAndFilling
{
    public partial class Form1 : Form
    {
        #region Properties
        // List of point for drawing a polygon
        public List<Point> polygon = new List<Point>();

        // End points for Drawing a line
        public Point pointA, pointB;

        // Set of 4 values defining edges of the rectangle
        public Rectangle rectangle = new Rectangle();


        // Structure storing rectangle
        public struct Rectangle
        {
            public int left;
            public int right;
            public int bottom;
            public int top;
        }

        // Line Class
        public class Line
        {
            public Point A;
            public Point B;

            public Line(Point a, Point b)
            {
                A = new Point(a.X, a.Y);
                B = new Point(b.X, b.Y);
            }
        }

        // Egde Class
        public class Edge
        {
            public int ymin;
            public int ymax;
            public float x;
            public float m_1;

            public Edge(int _ymin, int _ymax, float _x, float _m_1)
            {
                ymin = _ymin;
                ymax = _ymax;
                x = _x;
                m_1 = _m_1;
            }
        }


        // List of all lines
        public List<Line> lines = new List<Line>();


        // Intersected Points
        List<Point> ListOfIntersectPoints = new List<Point>();

        //
        int point_ymin = 2000;
        int point_ymax = 0;


        #endregion

        #region Utility
        /// <summary>
        /// Initialzie the form and declare empty image in the Drawing Area
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            pictureBoxDrawingArea.Image = new Bitmap(pictureBoxDrawingArea.Width, pictureBoxDrawingArea.Height);
        }

        /// <summary>
        /// Finding a minium of 2 integers
        /// </summary>
        private int min(int x, int y)
        {
            return (x < y) ? x : y;
        }

        /// <summary>
        /// Finding a maximum of 2 integers
        /// </summary>
        private int max(int x, int y)
        {
            return (x > y) ? x : y;
        }

        /// <summary>
        /// Assigning proper points for the rectangle
        /// </summary>
        private void BuildRectangle()
        {
            rectangle.right = max(pointA.X, pointB.X);
            rectangle.left = min(pointA.X, pointB.X);

            rectangle.top = max(pointA.Y, pointB.Y);
            rectangle.bottom = min(pointA.Y, pointB.Y);
        }


        /// <summary>
        /// Finding an outcode for a given rectangle
        /// </summary>
        private byte ComputeOutcode(Point p)
        {
            byte outcode = 0;
            if (p.X > rectangle.right) outcode = 2;
            else if (p.X < rectangle.left) outcode = 1;
            if (p.Y > rectangle.top) outcode = 8;
            else if (p.Y < rectangle.bottom) outcode = 4;
            return outcode;
        }

        #endregion

        #region Drawing Methods
        /// <summary>
        /// Handling  of cliking on the drawing area
        /// </summary>
        private void pictureBoxDrawingArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (checkBoxLineDrawing.Checked == true)
            {
                if (e.Button == MouseButtons.Right) pointB = new Point(e.X, e.Y);
                else pointA = new Point(e.X, e.Y);

                if (pointA.X != 0 && pointB.X != 0)
                {
                    lines.Add(new Line(pointA, pointB));
                    DrawLine(pointA, pointB);
                }

            }

            else if (checkBoxClipping.Checked == true)
            {

                if (e.Button == MouseButtons.Right) pointB = new Point(e.X, e.Y);
                else pointA = new Point(e.X, e.Y);

                if (pointA.X != 0 && pointB.X != 0)
                {
                    BuildRectangle();
                    pictureBoxDrawingArea.Image = new Bitmap(pictureBoxDrawingArea.Width, pictureBoxDrawingArea.Height);

                    foreach (Line l in lines)
                    {
                        CohenSutherland(l.A, l.B);
                        pictureBoxDrawingArea.Refresh();
                    }



                    pointA = new Point(0, 0);
                    pointB = new Point(0, 0);

                    lines.Clear();
                }

            }

            else if (checkBoxPolygon.Checked == true)
            {
                polygon.Add(new Point(e.X, e.Y));
            }


        }

        /// <summary>
        /// Implementation of CohenSutherland algorithm
        /// </summary>
        private void CohenSutherland(Point p1, Point p2)
        {
            Graphics g = Graphics.FromImage(pictureBoxDrawingArea.Image);
            bool accept = false, done = false;
            byte outcode1 = ComputeOutcode(p1);
            byte outcode2 = ComputeOutcode(p2);
            do
            {
                if ((outcode1 | outcode2) == 0)
                { //trivially accepted
                    accept = true;
                    done = true;
                }
                else if ((outcode1 & outcode2) != 0)
                { //trivially rejected
                    accept = false;
                    done = true;
                }

                else
                { //subdivide
                    byte outcodeOut = (outcode1 != 0) ? outcode1 : outcode2;
                    Point p = new Point();
                    if ((outcodeOut & 8) != 0)
                    {
                        p.X = p1.X + (p2.X - p1.X) * (rectangle.top - p1.Y) / (p2.Y - p1.Y);
                        p.Y = rectangle.top;
                    }
                    else if ((outcodeOut & 4) != 0)
                    {
                        p.X = p1.X + (p2.X - p1.X) * (rectangle.bottom - p1.Y) / (p2.Y - p1.Y);
                        p.Y = rectangle.bottom;
                    }
                    else if ((outcodeOut & 1) != 0)
                    {
                        p.Y = p1.Y + (p2.Y - p1.Y) * (rectangle.left - p1.X) / (p2.X - p1.X);
                        p.X = rectangle.left;
                    }
                    else if ((outcodeOut & 2) != 0)
                    {
                        p.Y = p1.Y + (p2.Y - p1.Y) * (rectangle.right - p1.X) / (p2.X - p1.X);
                        p.X = rectangle.right;
                    }

                    if (outcodeOut == outcode1)
                    {
                        p1 = p;
                        outcode1 = ComputeOutcode(p1);
                    }
                    else
                    {
                        p2 = p;
                        outcode2 = ComputeOutcode(p2);
                    }
                }
            } while (!done);
            if (accept) g.DrawLine(Pens.Black, p1, p2); ;

            g.Dispose();
            
        }


        /// <summary>
        /// Draw a line between 2 points
        /// </summary>
        private void DrawLine(Point _pointA, Point _pointB)
        {
            Graphics g = Graphics.FromImage(pictureBoxDrawingArea.Image);  
            g.DrawLine(Pens.Black, _pointA, _pointB);

            g.Dispose();
            pictureBoxDrawingArea.Refresh();

            pointA = new Point(0, 0);
            pointB = new Point(0, 0);
        }

    #endregion      

        #region CheckBoxex
        /// <summary>
        /// Checking disable Clipping and Polygon
        /// </summary>
        private void checkBoxLineDrawing_CheckedChanged(object sender, System.EventArgs e)
        {
            checkBoxClipping.Checked = false;
            checkBoxPolygon.Checked = false;
        }

        /// <summary>
        /// Checking clipping, disable LineDrawing and Polygon
        /// </summary>
        private void checkBoxClipping_CheckedChanged(object sender, System.EventArgs e)
        {
            checkBoxLineDrawing.Checked = false;
            checkBoxPolygon.Checked = false;
        }

        /// <summary>
        /// Checking polygon, disable LineDrawing and Clipping
        /// </summary>
        private void checkBoxPolygon_CheckedChanged(object sender, System.EventArgs e)
        {
            checkBoxClipping.Checked = false;
            checkBoxLineDrawing.Checked = false;
        }

        /// <summary>
        /// Draw a polygon selected by user
        /// </summary>
        public void DrawPoly()
        {
            Graphics g = Graphics.FromImage(pictureBoxDrawingArea.Image);

            int len = polygon.Count - 1;
            
            for (int i = 0; i<len; i++)
            {
                g.DrawLine(Pens.Black, polygon[i], polygon[i+1]);
            }

            g.DrawLine(Pens.Black, polygon[len], polygon[0]);

            g.Dispose();
            pictureBoxDrawingArea.Refresh();
        }

        #endregion

        #region Buttons
        /// <summary>
        /// Fill the polygon made by user
        /// </summary>
        private void buttonFill_Click(object sender, System.EventArgs e)
        {

            DrawPoly();

            Bitmap image = new Bitmap(pictureBoxDrawingArea.Image);
            List<Edge> edges = MakeET(polygon);
            List<Edge> global = MakeGET(edges);
            int y = global[0].ymin;
            List<Edge> active = new List<Edge>();
            bool even = true;
            int oldedgenumber = 0 , newedgenumber = 0;
            while (active.Any() || global.Any())
            {
                //move bucket ET[y] to AET
                foreach(Edge edge in global)
                {
                    if (edge.ymin == y) active.Add(edge);
                }

                foreach (Edge edge in active)
                {
                    if(global.Contains(edge))global.Remove(edge);
                }


                // sort AET by x value
                active = active.OrderBy(ed => ed.x).ToList();


                newedgenumber = active.Count;
                if (!even) even = true;
                if (newedgenumber != oldedgenumber && oldedgenumber != 0) even = false;
                int count = 0;
                int x = (int)active[count].x;               
                count++;

                bool parity = true;

                //fill pixels between pairs of intersections
                while(x<=active[active.Count - 1].x)
                {
                    if (parity) image.SetPixel(x, y, Color.Black);
                    if (count == active.Count) break;
                    
                    x++;
                    if(x == (int)active[count].x)
                    {
                        if(even)parity = !parity;
                        else
                        {

                        }
                      
                        count++;
                    }
                }

                oldedgenumber = newedgenumber;
                 ++y;

                //remove from AET edges for which ymax = y
                active.RemoveAll(a => a.ymax == y);


                foreach (Edge edge in active)
                {
                    edge.x = edge.x + edge.m_1;
                }

                pictureBoxDrawingArea.Image = image;
                pictureBoxDrawingArea.Refresh();

            }
            

        }

        public List<Edge> MakeAET(List<Edge> edges)
        {
            List<Edge> AET = new List<Edge>();


            return AET;
        }

        public List<Edge> MakeGET(List<Edge> edges)
        {
            List<Edge> ordered = edges.OrderBy(min => min.ymin).ThenBy(max => max.ymax).ThenBy(p => p.x).ToList();
            ordered.RemoveAll(a => a.m_1 == Int32.MaxValue);
            return ordered;
        }

        public void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public List<Edge> MakeET(List<Point> points)
        {
            int len = points.Count;
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < len - 1; i++)
            {
                int _ymin, _ymax;
                float _m, _x;
                int _y1 = points[i].Y;
                int _y2 = points[i + 1].Y;
                int _x1 = points[i].X;
                int _x2 = points[i + 1].X;

                if(_y2 > _y1)
                {
                    _ymax = _y2;
                    _ymin = _y1;
                    _x = _x1;
                }              
                else
                {
                    _ymax = _y1;
                    _ymin = _y2;
                    _x = _x2;
                }

                //_x = min(_x1, _x2);

                _m = (float)((float)(_x2 - _x1) / (float)(_y2 - _y1));

                //if (_m < 0 && _x == _x1) _x = _x2;
                //else if (_m < 0 && _x == _x2) _x = _x1;

                if ((_y2 - _y1) != 0)
                {
                    edges.Add(new Edge(_ymin, _ymax, _x, _m));

                }
                else
                {
                    edges.Add(new Edge(_ymin, _ymax, _x, Int32.MaxValue));
                }


            }

            int ymin, ymax;
            float x, m;
            int y1 = points[len - 1].Y;
            int y2 = points[0].Y;
            int x1 = points[len - 1].X;
            int x2 = points[0].X;

            if (y2 > y1)
            {
                ymax = y2;
                ymin = y1;
                x = x1;
            }
            else
            {
                ymax = y1;
                ymin = y2;
                x = x2;
            }

            //x = min(x1, x2);

            m = (float)((float)(x2 - x1) / (float)(y2 - y1));

            //if (m < 0 && x == x1) x = x2;
            //else if (m < 0 && x == x2) x = x1;

            if ((y2 - y1) != 0)
            {
                edges.Add(new Edge(ymin, ymax, x, m));
            }
            else
            {
                edges.Add(new Edge(ymin, ymax, x, Int32.MaxValue));
            }
            return edges;

        }

        /// <summary>
        /// Clear Drawing Area
        /// </summary>
        private void buttonReset_Click(object sender, System.EventArgs e)
        {
            pictureBoxDrawingArea.Image = new Bitmap(pictureBoxDrawingArea.Width, pictureBoxDrawingArea.Height);
            lines.Clear();
            polygon.Clear();
            pointA = new Point(0, 0);
            pointB = new Point(0, 0);
        }

        #endregion       
    }
}
