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
        Error,
        Popularity_algorithm,
        k_means_color_quantization
    };
    public partial class Dithering : Form
    {
        int imageWidth, imageHeight, greyvalueSum, K = 2;

        //private 
        private Bitmap AverageDithering(Bitmap outputImage)
        {
            int[] tresholds = new int[K];
            int ratio = greyvalueSum / (imageHeight * imageWidth);
            for (int i = 0; i < tresholds.Length / 2; i++)
            {
                tresholds[i] = 2 * i * ratio / K;
            }

            for (int i = 0; i < tresholds.Length / 2; i++)
            {
                tresholds[i + tresholds.Length / 2] = ratio + 2 * i * (255 - ratio) / K;
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

        private Bitmap ErrorDiffusion(Bitmap outputImage)
        {
            return outputImage;
        }

        private int[,,] MakeColorCube(Bitmap outputImage)
        {
            int[,,] cube = new int[64, 64, 64];

            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    for (int k = 0; k < 64; k++)
                    {
                        cube[i, j, k] = 0;
                    }
                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    Color pixel = outputImage.GetPixel(x, y);
                    cube[pixel.R / 4, pixel.G / 4, pixel.B / 4]++;
                }
            }
            return cube;
        }

        public class Vector
        {

            public int X;
            public int Y;
            public int Z;
            public int Value;

            public Vector(int x = 0, int y = 0, int z = 0, int value = 0)
            {
                X = x;
                Y = y;
                Z = z;
                Value = value;
            }

        }

        public class RGB
        {
            public int R;
            public int G;
            public int B;
            public RGB(int r = 0, int g = 0, int b = 0)
            {
                R = r;
                G = g;
                B = b;
            }
        }


        private List<Vector> FindMaximaOfCube(int[,,] cube)
        {
            List<Vector> maxima = new List<Vector>();


            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    for (int k = 0; k < 64; k++)
                    {
                        if (maxima.Count != K)
                        {
                            maxima.Add(new Vector(i, j, k, cube[i, j, k]));
                        }

                        else
                        {
                            foreach (Vector vector in maxima)
                            {
                                if (cube[i, j, k] > vector.Value)
                                {
                                    maxima.Remove(vector);
                                    maxima.Add(new Vector(i, j, k, cube[i, j, k]));
                                    break;
                                }
                            }

                        }
                    }
                }
            }

            return maxima;
        }

        private List<RGB> MakePalette(List<Vector> maxima)
        {
            List<RGB> palette = new List<RGB>();

            foreach (Vector vector in maxima)
            {
                palette.Add(new RGB(vector.X * 4, vector.Y * 4, vector.Z * 4));
            }

            return palette;
        }

        private double DistanceBetweenColors(int x1, int x2, int x3, int y1, int y2, int y3)
        {
            int a = (x1 - y1);
            int b = (x2 - y2);
            int c = (x3 - y3);

            return (a * a + b * b + c * c);
        }

        private RGB FindMinimalDistance(List<RGB> palette, Color color)
        {
            double min = 3 * 256 * 256;
            RGB minimal = new RGB();
            foreach (RGB paletteColor in palette)
            {
                double distance = DistanceBetweenColors(paletteColor.R, paletteColor.G, paletteColor.B, color.R, color.G, color.B);
                if (min > distance)
                {
                    min = distance;
                    minimal = new RGB(paletteColor.R, paletteColor.G, paletteColor.B);
                }
            }
            return minimal;
        }


        private Bitmap PopularityColorQuantization(Bitmap outputImage)
        {
            int[,,] cube = MakeColorCube(outputImage);
            List<Vector> maxima = FindMaximaOfCube(cube);
            List<RGB> palette = MakePalette(maxima);

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    Color pixel = outputImage.GetPixel(x, y);
                    RGB newPixel = FindMinimalDistance(palette, pixel);

                    outputImage.SetPixel(x, y, Color.FromArgb(newPixel.R, newPixel.G, newPixel.B));
                }
            }


            return outputImage;
        }

        public class Centroid : RGB
        {
            public List<RGB> pixels;

            public Centroid(int R = 0, int G = 0, int B = 0, List<RGB> p = null) : base(R, G, B)
            {

                pixels = new List<RGB>(p);
            }
        }

        public List<Centroid> FindCentroids()
        {
            return new List<Centroid>();
        }

        public List<Centroid> RandomizeCentroids()
        {
            List<Centroid> centroids = new List<Centroid>();

            for (int i = 0; i<K; i++)
            {
                Random random = new Random();
                int R = random.Next(0, 256);
                int G = random.Next(0, 256);
                int B = random.Next(0, 256);


                centroids.Add(new Centroid(R, G, B, null));
            }

            return centroids;
        }

        public RGB[,] ConvertBitmapToRGB(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            RGB[,] imageInRGB = new RGB[imageWidth, imageHeight];

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    imageInRGB[x, y] = new RGB(pixel.R, pixel.G, pixel.B);
                }
            }

            return imageInRGB;
        }

        private Centroid FindMinimalDistance(List<Centroid> centroids, RGB color)
        {
            double min = 3 * 256 * 256;
            Centroid minimal = new Centroid();
            foreach (Centroid centroid in centroids)
            {
                double distance = DistanceBetweenColors(centroid.R, centroid.G, centroid.B, color.R, color.G, color.B);
                if (min > distance)
                {
                    min = distance;
                    //minimal = new RGB(paletteColor.R, paletteColor.G, paletteColor.B);
                }
            }
            return minimal;
        }

        public void AssignPixlesToCentroid(List<Centroid> centroids, RGB[,] image)
        {
            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    
                }
            }
            
        }

        private Bitmap kMeansColorQuantization(Bitmap outputImage)
        {
            RGB[,] imageInPixles = ConvertBitmapToRGB(outputImage);
            List<Centroid> centroids = RandomizeCentroids();






            return outputImage;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            Bitmap outputImage = new Bitmap(pictureBox2.Image);

            if (kText.Text == "") ;
            else Int32.TryParse(kText.Text, out K);

            switch (algorithmCombo.SelectedItem)
            {
                case Algorithms.Error:

                    ErrorDiffusion(outputImage);
                    break;

                case Algorithms.k_means_color_quantization:
                    outputImage = new Bitmap(pictureBox1.Image);
                    kMeansColorQuantization(outputImage);
                    break;

                case Algorithms.Popularity_algorithm:
                    outputImage = new Bitmap(pictureBox1.Image);
                    PopularityColorQuantization(outputImage);
                    
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
            algorithmCombo.Items.Add(Algorithms.k_means_color_quantization);
            algorithmCombo.Items.Add(Algorithms.Popularity_algorithm);

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

                outputImage = AverageDithering(outputImage);

                pictureBox3.Image = outputImage;

                }
            }
        
    }
}
