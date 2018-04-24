﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_03_Rasterization
{
    public partial class Form1 : Form
    {
        int dots = 0, radius = 50;
        public Form1()
        {
            InitializeComponent();
            pictureBoxDrawArea.Image = new Bitmap(pictureBoxDrawArea.Width, pictureBoxDrawArea.Height);
        }

        private void checkBoxCircle_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxLine.Checked = false;
            dots = 0;
        }

        private void checkBoxLine_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxCircle.Checked = false;
            dots = 0;
        }

        private void pictureBoxDrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBoxDrawArea.Image);

            if (checkBoxLine.Checked == true)
            {
                dots++;
                if (dots < 2)
                {
                    bitmap.SetPixel(e.X, e.Y, Color.Black);

                    pictureBoxDrawArea.Tag = new Point(e.X, e.Y);
                }
                else
                {
                    if (checkBoxMarker.Checked == true)
                    {
                        WuLine(e.X, e.Y, ((Point)pictureBoxDrawArea.Tag).X, ((Point)pictureBoxDrawArea.Tag).Y, bitmap);
                    }
                    else
                    {
                        DrawSymmetricLine(e.X, e.Y, ((Point)pictureBoxDrawArea.Tag).X, ((Point)pictureBoxDrawArea.Tag).Y, bitmap);                      
                    }
                    dots = 0;

                }
            }

            else if (checkBoxCircle.Checked == true)
            {
                if (checkBoxMarker.Checked == true)
                {
                    WuCircle(e.X, e.Y, radius, bitmap);
                }
                else
                {
                    MidpointCircle(e.X, e.Y, radius, bitmap);
                }
            }

            pictureBoxDrawArea.Image = bitmap;
        }
        


    

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            pictureBoxDrawArea.Image = new Bitmap(pictureBoxDrawArea.Width, pictureBoxDrawArea.Height);
        }

        private void DrawSymmetricLine(int x1, int y1, int x2, int y2, Bitmap bitmap)
        {
            if(x1 > x2)
            {
                int tempX = x1, tempY = y1;
                x1 = x2;
                y1 = y2;
                x2 = tempX;
                y2 = tempY;
            }

            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = 2 * dy - dx;
            int dE = 2 * dy;
            int dNE = 2 * (dy - dx);
            int xf = x1, yf = y1;
            int xb = x2, yb = y2;
            bitmap.SetPixel(xf, yf, Color.Black);
            bitmap.SetPixel(xb, yb, Color.Black);
            while (xf < xb)
            {
                ++xf; --xb;
                if (d < 0)
                    d += dE;
                else
                {
                    d += dNE;
                    ++yf;
                    --yb;
                }
                bitmap.SetPixel(xf, yf, Color.Black);
                bitmap.SetPixel(xb, yb, Color.Black);
            }
        }
        void MidpointCircle(int x0, int y0, int R, Bitmap bitmap)
        {
            int dE = 3;
            int dSE = 5 - 2 * R;
            int d = 1 - R;
            int x = 0;
            int y = R;
            bitmap.SetPixel(x + x0, y + y0, Color.Black);
            bitmap.SetPixel(x + x0, -y + y0, Color.Black);
            bitmap.SetPixel(-x + x0, y + y0, Color.Black);
            bitmap.SetPixel(-x + x0, -y + y0, Color.Black);
            bitmap.SetPixel(y + x0, x + y0, Color.Black);
            bitmap.SetPixel(y + x0, -x + y0, Color.Black);
            bitmap.SetPixel(-y + x0, x + y0, Color.Black);
            bitmap.SetPixel(-y + x0, -x + y0, Color.Black);
            while (y > x)
            {
                if (d < 0) //move to E
                {
                    d += dE;
                    dE += 2;
                    dSE += 2;
                }
                else //move to SE
                {
                    d += dSE;
                    dE += 2;
                    dSE += 4;
                    --y;
                }
                ++x;
                bitmap.SetPixel(x + x0, y + y0, Color.Black);
                bitmap.SetPixel(x + x0, -y + y0, Color.Black);
                bitmap.SetPixel(-x + x0, y + y0, Color.Black);
                bitmap.SetPixel(-x + x0, -y + y0, Color.Black);
                bitmap.SetPixel(y + x0, x + y0, Color.Black);
                bitmap.SetPixel(y + x0, -x + y0, Color.Black);
                bitmap.SetPixel(-y + x0, x + y0, Color.Black);
                bitmap.SetPixel(-y + x0, -x + y0, Color.Black);
            }
        }
        void WuLine(int x1, int y1, int x2, int y2, Bitmap bitmap)
        {
            float L = 0; /*Line color*/
            float B = 200; /*Background Color*/
            float m = ((float)y2 - (float)y1) / ((float)x2 - (float)x1);

            if (x1 > x2)
            {
                int tempX = x1, tempY = y1;
                x1 = x2;
                y1 = y2;
                x2 = tempX;
                y2 = tempY;
            }


            int x;
            float y = y1;
            for (x = x1; x <= x2; ++x)
            {
                float c1 = L * (1 - modf(y)) + B * modf(y);
                float c2 = L * modf(y) + B * (1 - modf(y));
                bitmap.SetPixel(x, (int)y, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(x, (int)y + 1, Color.FromArgb((int)c2, (int)c2, (int)c2));
                
                y += m;
            }
        }
        {
            int L = 0; /*Line color*/
            int B = 200; /*Background Color*/
            int x = R;
            int y = 0;
            bitmap.SetPixel(x + x0, y + y0, Color.FromArgb(L,L,L));
            bitmap.SetPixel(x + x0, -y + y0, Color.FromArgb(L, L, L));
            bitmap.SetPixel(-x + x0, y + y0, Color.FromArgb(L, L, L));
            bitmap.SetPixel(-x + x0, -y + y0, Color.FromArgb(L, L, L));
            bitmap.SetPixel(y + x0, x + y0, Color.FromArgb(L, L, L));
            bitmap.SetPixel(y + x0, -x + y0, Color.FromArgb(L, L, L));
            bitmap.SetPixel(-y + x0, x + y0, Color.FromArgb(L, L, L));
            bitmap.SetPixel(-y + x0, -x + y0, Color.FromArgb(L, L, L));
            while (x > y)
            {
                ++y;
                x = (int)(Math.Sqrt(R * R - y * y)) + 1;
                float T = (float)x - (float)(Math.Sqrt(R * R - y * y));
                float c2 = L * (1 - T) + B * T;
                float c1 = L * T + B * (1 - T);
                bitmap.SetPixel(x + x0, y + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));
                bitmap.SetPixel(x + x0, -y + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));
                bitmap.SetPixel(-x + x0, y + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));
                bitmap.SetPixel(-x + x0, -y + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));
                bitmap.SetPixel(y + x0, x + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));
                bitmap.SetPixel(y + x0, -x + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));
                bitmap.SetPixel(-y + x0, x + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));
                bitmap.SetPixel(-y + x0, -x + y0, Color.FromArgb((int)c2, (int)c2, (int)c2));

                bitmap.SetPixel(x - 1 + x0, y + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(x - 1 + x0, -y + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(-x + 1 + x0, y + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(-x + 1 + x0, -y + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(y - 1 + x0, x + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(y - 1 + x0, -x + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(-y + 1 + x0, x + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
                bitmap.SetPixel(-y + 1 + x0, -x + y0, Color.FromArgb((int)c1, (int)c1, (int)c1));
            }
        }
        {
            return x - (int)x;
        }
    }
}