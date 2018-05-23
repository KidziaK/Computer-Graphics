using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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

        // Strucutre storing 2 points representing lines
        public struct Line
        {
            public Point A;
            public Point B;

            public Line(Point a, Point b)
            {
                A = new Point(a.X, a.Y);
                B = new Point(b.X, b.Y);
            }
        }

        // List of all lines
        public List<Line> lines = new List<Line>();

        // Active edge table implementation
        List<Line> AET = new List<Line>();

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

        #endregion

        #region Buttons
        /// <summary>
        /// Fill the polygon made by user
        /// </summary>
        private void buttonFill_Click(object sender, System.EventArgs e)
        {
            int edgesSize = AET.Count;
            for (int i = point_ymin; i <= point_ymax; i++)
            {
                int intersectX = 0;// co tu zadeklarowac
                for (int j = 0; j < edgesSize; j++)
                {
                    if (findIntersectPoint(AET[j].A.X, AET[j].A.Y, AET[j].B.X, AET[j].B.Y, i, ref intersectX))
                    {
                        Point p = new Point(intersectX, i);
                        if (AET[j].A.Y > AET[j].B.Y)
                        {
                            if (p.Y == AET[j].A.Y)
                                continue;
                        }
                        else
                            if (AET[j].A.Y < AET[j].B.Y)
                        {
                            if (p.Y == AET[j].B.Y)
                                continue;
                        }
                        ListOfIntersectPoints.Add(p);
                    }
                }
                int intersectSize = ListOfIntersectPoints.Count;
                Point swap = new Point(0, 0);
                for (int k = 0; k < intersectSize - 1; k++)
                    for (int j = k + 1; j < intersectSize; j++)
                    {
                        if (ListOfIntersectPoints[k].X > ListOfIntersectPoints[j].X)
                        {
                            swap = ListOfIntersectPoints[k];
                            ListOfIntersectPoints[k] = ListOfIntersectPoints[j];
                            ListOfIntersectPoints[j] = swap;
                        }
                    }
                int intersectPointsSize = ListOfIntersectPoints.Count;
                for (int j = 1; j < intersectPointsSize; j += 2)
                {

                    //glBegin(GL_LINES);
                    //glVertex2i(ListOfIntersectPoints.at(j - 1).x, ListOfIntersectPoints.at(j - 1).y);
                    //glVertex2i(ListOfIntersectPoints.at(j).x, ListOfIntersectPoints.at(j).y);
                    //glEnd();

                }
                ListOfIntersectPoints.Clear();
            }
        }
        bool findIntersectPoint(int x1, int y1, int x2, int y2, int y, ref int x)
        {
            if (y2 == y1)
                return false;
            x = (x2 - x1) * (y - y1) / (y2 - y1) + x1;
            bool isInsideEdgeX;
            bool isInsideEdgeY;
            if (x1 < x2)
                isInsideEdgeX = (x1 <= x) && (x <= x2);
            else
                isInsideEdgeX = (x2 <= x) && (x <= x1);

            if (y1 < y2)
                isInsideEdgeY = (y1 <= y) && (y <= y2);
            else
                isInsideEdgeY = (y2 <= y) && (y <= y1);
            return isInsideEdgeX && isInsideEdgeY;
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
