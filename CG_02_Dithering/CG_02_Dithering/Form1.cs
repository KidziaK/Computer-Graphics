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
    public enum Algorithms
    {
        Average,
        Error
    };
    public partial class Dithering : Form
    {
        int imageWidth, imageHeight, greyvalueSum, K = 2;

        private Bitmap AverageDithering(Bitmap outputImage)
        {
            int[] tresholds = new int[K];
            int ratio = greyvalueSum / (imageHeight * imageWidth * (int)Math.Log((double)K, 2.0));
            for (int i = 0; i < tresholds.Length; i++)
            {
                tresholds[i] = i * ratio;
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    int value = 255;
                    Color pixel = outputImage.GetPixel(x, y);

                    for (int i = 1; i < tresholds.Length; i++)
                    {
                        if (pixel.R < tresholds[i] && pixel.R >= tresholds[i - 1])
                        {
                            value = (i - 1) * (255 / (tresholds.Length - 1));
                            break;
                        }
                    }

                    outputImage.SetPixel(x, y, Color.FromArgb(value, value, value));
                }
            }
            return outputImage;
        }
        private void applyButton_Click(object sender, EventArgs e)
        {
            Bitmap outputImage = new Bitmap(pictureBox2.Image);

            if (kText.Text == "") ;
            else Int32.TryParse(kText.Text, out K);

            switch (algorithmCombo.SelectedItem)
            {
                case Algorithms.Error:

                    break;

                default:
                    outputImage = AverageDithering(outputImage);
                    break;

            }

            pictureBox3.Image = outputImage;


        }

        public Dithering()
        {
            InitializeComponent();
            algorithmCombo.Items.Add(Algorithms.Average);
            algorithmCombo.Items.Add(Algorithms.Error);

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
                imageHeight = outputImage.Height;
                imageWidth = outputImage.Width;
                greyvalueSum = 0;
                for (int x = 0; x < imageWidth; x++)
                {
                    for (int y = 0; y < imageHeight; y++)
                    {

                        Color pixel = outputImage.GetPixel(x, y);
                        int greyValue = (int)(0.3 * (double)pixel.G + 0.6 * (double)pixel.R + 0.1 * (double)pixel.B);

                        greyvalueSum += greyValue;
                        outputImage.SetPixel(x, y, Color.FromArgb(greyValue, greyValue, greyValue));
                    }
                }
                pictureBox2.Image = outputImage;
                outputImage = new Bitmap(pictureBox2.Image);
                if (kText.Text == "") ;
                else Int32.TryParse(kText.Text, out K);

                switch (algorithmCombo.SelectedItem)
                {
                    case Algorithms.Error:
                        
                        break;

                    default:
                        outputImage = AverageDithering(outputImage);
                        break;
                }

                pictureBox3.Image = outputImage;

                }
            }
        
    }
}
