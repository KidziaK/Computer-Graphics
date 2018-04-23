using System;
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
        int dots = 0, radius = 1;
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
                    DrawSymmetricLine(e.X, e.Y, ((Point)pictureBoxDrawArea.Tag).X, ((Point)pictureBoxDrawArea.Tag).Y, bitmap);
                    
                    dots = 0;

                }
            }

            else if (checkBoxCircle.Checked == true)
            {
                bitmap.SetPixel(e.X, e.Y, Color.Black);
                MidpointCircle(e.X, e.Y, radius, bitmap);
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
            int x = x0;
            int y = y0 + R;
            bitmap.SetPixel(x, y, Color.Black);
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
                bitmap.SetPixel(x, y, Color.Black);
            }
        }
    }
}
