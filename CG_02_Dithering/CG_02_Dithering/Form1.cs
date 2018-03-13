using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_02_Dithering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select a picture";
                dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(dlg.FileName);

                    
                }

                Bitmap outputImage = new Bitmap(pictureBox1.Image);
                int height = outputImage.Height;
                int width = outputImage.Width;
                int sum = 0;
                for (int x = 0; x < outputImage.Width; x++)
                {
                    for (int y = 0; y < outputImage.Height; y++)
                    {

                        Color pixel = outputImage.GetPixel(x, y);
                        int greyValue = (int)(0.3 * (double)pixel.G + 0.6 * (double)pixel.R + 0.1 * (double)pixel.B);

                        sum += greyValue;
                        outputImage.SetPixel(x, y, Color.FromArgb(greyValue, greyValue, greyValue));
                    }
                }
                pictureBox2.Image = outputImage;

                outputImage = new Bitmap(pictureBox2.Image);
                int treshold = sum / (height * width);

                for (int x = 0; x < outputImage.Width; x++)
                {
                    for (int y = 0; y < outputImage.Height; y++)
                    {
                            int red, blue, green;
                        Color pixel = outputImage.GetPixel(x, y);
                        if(pixel.R < treshold)
                            {
                                red = 0;
                                green = 0;
                                blue = 0;
                            }
                            else
                            {
                                red = 255;
                                green = 255;
                                blue = 255;
                            }
                        outputImage.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
            pictureBox3.Image = outputImage;
                }
            }
        
    }
}
