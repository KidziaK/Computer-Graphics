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
    public enum Matrices
    {
        FloydSteinberg,
        Burkes,
        Stucky,
        Sierra,
        Atkinson
    };
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
        public Vector(Vector vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
            Value = vector.Value;
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

        public RGB(Vector vector)
        {
            R = vector.X;
            G = vector.Y;
            B = vector.Z;
        }
    }
    public class Centroid : RGB
    {
        public List<RGB> pixels;

        public Centroid(int R = 0, int G = 0, int B = 0, List<RGB> p = null) : base(R, G, B)
        {

            pixels = new List<RGB>(p);
        }
    }
    public partial class Dithering : Form
    {
        int imageWidth, imageHeight, greyvalueSum, K = 2;
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

        private int FindMinimalGreyDistance(List<int> greyPalette, int pixel)
        {
            int minimalDistance = 255, minimalColor = 0;
            

            foreach (int color in greyPalette)
            {
                int distance = Math.Abs(color - pixel);
                if(minimalDistance > distance)
                {
                    minimalDistance = distance;
                    minimalColor = color;
                }
            }

            return minimalColor;
        }

        private int[,] ConvertBitMapToGreyImage(Bitmap image)
        {
            int[,] greyImage = new int[imageWidth, imageHeight];

            for(int x = 0; x<imageWidth; x++)
            {
                for(int y = 0; y<imageHeight; y++)
                {
                    greyImage[x,y] = image.GetPixel(x, y).R;
                }
            }
            return greyImage;
        }

        private Bitmap FloydSteinberg(Bitmap outputImage)
        {
            List<int> greyPalette = new List<int>();
            int[,] greyImage = ConvertBitMapToGreyImage(outputImage);

            int step = 255 / (K-1);
            for(int i = 0; i<K; i++)
            {
                greyPalette.Add(i * step);
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    int pixel = greyImage[x, y];
                    int newColor = FindMinimalGreyDistance(greyPalette, pixel);
                    int difference = pixel - newColor;
                    greyImage[x, y] = newColor;

                    if (x + 1 < imageWidth)
                    {
                        if (greyImage[x + 1, y] + (int)(7 * (double)difference / 16) > 0 && greyImage[x + 1, y] + (int)(7 * (double)difference / 16) <= 255) greyImage[x + 1, y] = greyImage[x + 1, y] + (int)(7 * (double)difference / 16);
                        else greyImage[x + 1, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y]); ;
                    }

                    if (x - 1 > 0 && y + 1 < imageHeight)
                    {
                        if (greyImage[x - 1, y + 1] + (int)(3 * (double)difference / 16) > 0 && greyImage[x - 1, y + 1] + (int)(3 * (double)difference / 16) <= 255) greyImage[x - 1, y + 1] = greyImage[x - 1, y + 1] + (int)(3 * (double)difference / 16);
                        else greyImage[x - 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x - 1, y + 1]);
                    }

                    if (y + 1 < imageHeight)
                    {
                        if (greyImage[x, y + 1] + (int)(5 * (double)difference / 16) > 0 && greyImage[x, y + 1] + (int)(5 * (double)difference / 16) <= 255) greyImage[x, y + 1] = greyImage[x, y + 1] + (int)(5 * (double)difference / 16);
                        else greyImage[x, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x, y + 1]);
                    }

                    if (x + 1 < imageWidth && y + 1 < imageHeight)
                    {
                        if (greyImage[x + 1, y + 1] + (int)((double)difference / 16) > 0 && greyImage[x + 1, y + 1] + (int)((double)difference / 16) <= 255)  greyImage[x + 1, y + 1] = greyImage[x + 1, y + 1] + (int)((double)difference / 16);
                        else greyImage[x + 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y + 1]);
                    }

                    
                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    outputImage.SetPixel(x, y, Color.FromArgb(greyImage[x, y], greyImage[x, y], greyImage[x, y]));
                }
            }


            return outputImage;
        }

        private Bitmap Atkinson(Bitmap outputImage)
        {
            List<int> greyPalette = new List<int>();
            int[,] greyImage = ConvertBitMapToGreyImage(outputImage);

            int step = 255 / (K - 1);
            for (int i = 0; i < K; i++)
            {
                greyPalette.Add(i * step);
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    int pixel = greyImage[x, y];
                    int newColor = FindMinimalGreyDistance(greyPalette, pixel);
                    int difference = pixel - newColor;
                    greyImage[x, y] = newColor;

                    if (x + 1 < imageWidth)
                    {
                        if (greyImage[x + 1, y] + (int)(1 * (double)difference / 8) > 0 && greyImage[x + 1, y] + (int)(1 * (double)difference / 8) <= 255) greyImage[x + 1, y] = greyImage[x + 1, y] + (int)(1 * (double)difference / 8);
                        else greyImage[x + 1, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y]); ;
                    }

                    if (x + 2 < imageWidth)
                    {
                        if (greyImage[x + 2, y] + (int)(1 * (double)difference / 8) > 0 && greyImage[x + 2, y] + (int)(1 * (double)difference / 8) <= 255) greyImage[x + 2, y] = greyImage[x + 2, y] + (int)(1 * (double)difference / 8);
                        else greyImage[x + 2, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 2, y]); ;
                    }

                    if (x - 1 > 0 && y + 1 < imageHeight)
                    {
                        if (greyImage[x - 1, y + 1] + (int)(1 * (double)difference / 8) > 0 && greyImage[x - 1, y + 1] + (int)(1 * (double)difference / 8) <= 255) greyImage[x - 1, y + 1] = greyImage[x - 1, y + 1] + (int)(1 * (double)difference / 8);
                        else greyImage[x - 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x - 1, y + 1]);
                    }

                    if (y + 1 < imageHeight)
                    {
                        if (greyImage[x, y + 1] + (int)(1 * (double)difference / 8) > 0 && greyImage[x, y + 1] + (int)(1 * (double)difference / 8) <= 255) greyImage[x, y + 1] = greyImage[x, y + 1] + (int)(1 * (double)difference / 8);
                        else greyImage[x, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x, y + 1]);
                    }

                    if (y + 2 < imageHeight)
                    {
                        if (greyImage[x, y + 2] + (int)(1 * (double)difference / 8) > 0 && greyImage[x, y + 2] + (int)(1 * (double)difference / 8) <= 255) greyImage[x, y + 2] = greyImage[x, y + 2] + (int)(1 * (double)difference / 8);
                        else greyImage[x, y + 2] = FindMinimalGreyDistance(greyPalette, greyImage[x, y + 2]);
                    }

                    if (x + 1 < imageWidth && y + 1 < imageHeight)
                    {
                        if (greyImage[x + 1, y + 1] + (int)((double)difference / 8) > 0 && greyImage[x + 1, y + 1] + (int)((double)difference / 8) <= 255) greyImage[x + 1, y + 1] = greyImage[x + 1, y + 1] + (int)((double)difference / 8);
                        else greyImage[x + 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y + 1]);
                    }


                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    outputImage.SetPixel(x, y, Color.FromArgb(greyImage[x, y], greyImage[x, y], greyImage[x, y]));
                }
            }


            return outputImage;
        }

        private Bitmap Burkes(Bitmap outputImage)
        {
            List<int> greyPalette = new List<int>();
            int[,] greyImage = ConvertBitMapToGreyImage(outputImage);

            int step = 255 / (K - 1);
            for (int i = 0; i < K; i++)
            {
                greyPalette.Add(i * step);
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    int pixel = greyImage[x, y];
                    int newColor = FindMinimalGreyDistance(greyPalette, pixel);
                    int difference = pixel - newColor;
                    greyImage[x, y] = newColor;

                    if (x + 1 < imageWidth)
                    {
                        if (greyImage[x + 1, y] + (int)(8 * (double)difference / 32) > 0 && greyImage[x + 1, y] + (int)(8 * (double)difference / 32) <= 255) greyImage[x + 1, y] = greyImage[x + 1, y] + (int)(8 * (double)difference / 32);
                        else greyImage[x + 1, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y]); ;
                    }

                    if (x + 2 < imageWidth)
                    {
                        if (greyImage[x + 2, y] + (int)(4 * (double)difference / 32) > 0 && greyImage[x + 2, y] + (int)(4 * (double)difference / 32) <= 255) greyImage[x + 2, y] = greyImage[x + 2, y] + (int)(4 * (double)difference / 32);
                        else greyImage[x + 2, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 2, y]); ;
                    }

                    if (x - 1 > 0 && y + 1 < imageHeight)
                    {
                        if (greyImage[x - 1, y + 1] + (int)(4 * (double)difference / 32) > 0 && greyImage[x - 1, y + 1] + (int)(4 * (double)difference / 32) <= 255) greyImage[x - 1, y + 1] = greyImage[x - 1, y + 1] + (int)(4 * (double)difference / 32);
                        else greyImage[x - 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x - 1, y + 1]);
                    }

                    if (x - 2 > 0 && y + 1 < imageHeight)
                    {
                        if (greyImage[x - 2, y + 1] + (int)(2 * (double)difference / 32) > 0 && greyImage[x - 2, y + 1] + (int)(2 * (double)difference / 32) <= 255) greyImage[x - 2, y + 1] = greyImage[x - 2, y + 1] + (int)(2 * (double)difference / 32);
                        else greyImage[x - 2, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x - 2, y + 1]);
                    }

                    if (y + 1 < imageHeight)
                    {
                        if (greyImage[x, y + 1] + (int)(8 * (double)difference / 32) > 0 && greyImage[x, y + 1] + (int)(8 * (double)difference / 32) <= 255) greyImage[x, y + 1] = greyImage[x, y + 1] + (int)(8 * (double)difference / 32);
                        else greyImage[x, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x, y + 1]);
                    }

                    if (x + 1 < imageWidth && y + 1 < imageHeight)
                    {
                        if (greyImage[x + 1, y + 1] + (int)(4* (double)difference / 32) > 0 && greyImage[x + 1, y + 1] + (int)(4* (double)difference / 32) <= 255) greyImage[x + 1, y + 1] = greyImage[x + 1, y + 1] + (int)(4* (double)difference / 32);
                        else greyImage[x + 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y + 1]);
                    }

                    if (x + 2 < imageWidth && y + 1 < imageHeight)
                    {
                        if (greyImage[x + 2, y + 1] + (int)(2 * (double)difference / 32) > 0 && greyImage[x + 2, y + 1] + (int)(2 * (double)difference / 32) <= 255) greyImage[x + 2, y + 1] = greyImage[x + 2, y + 1] + (int)(2 * (double)difference / 32);
                        else greyImage[x + 2, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x + 2, y + 1]);
                    }

                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    outputImage.SetPixel(x, y, Color.FromArgb(greyImage[x, y], greyImage[x, y], greyImage[x, y]));
                }
            }
            return outputImage;
        }

        private Bitmap Sierra(Bitmap outputImage)
        {
            List<int> greyPalette = new List<int>();
            int[,] greyImage = ConvertBitMapToGreyImage(outputImage);

            int step = 255 / (K - 1);
            for (int i = 0; i < K; i++)
            {
                greyPalette.Add(i * step);
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    int pixel = greyImage[x, y];
                    int newColor = FindMinimalGreyDistance(greyPalette, pixel);
                    int difference = pixel - newColor;
                    greyImage[x, y] = newColor;

                    if (x + 1 < imageWidth)
                    {
                        if (greyImage[x + 1, y] + (int)(7 * (double)difference / 16) > 0 && greyImage[x + 1, y] + (int)(7 * (double)difference / 16) <= 255) greyImage[x + 1, y] = greyImage[x + 1, y] + (int)(7 * (double)difference / 16);
                        else greyImage[x + 1, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y]); ;
                    }

                    if (x - 1 > 0 && y + 1 < imageHeight)
                    {
                        if (greyImage[x - 1, y + 1] + (int)(3 * (double)difference / 16) > 0 && greyImage[x - 1, y + 1] + (int)(3 * (double)difference / 16) <= 255) greyImage[x - 1, y + 1] = greyImage[x - 1, y + 1] + (int)(3 * (double)difference / 16);
                        else greyImage[x - 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x - 1, y + 1]);
                    }

                    if (y + 1 < imageHeight)
                    {
                        if (greyImage[x, y + 1] + (int)(5 * (double)difference / 16) > 0 && greyImage[x, y + 1] + (int)(5 * (double)difference / 16) <= 255) greyImage[x, y + 1] = greyImage[x, y + 1] + (int)(5 * (double)difference / 16);
                        else greyImage[x, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x, y + 1]);
                    }

                    if (x + 1 < imageWidth && y + 1 < imageHeight)
                    {
                        if (greyImage[x + 1, y + 1] + (int)((double)difference / 16) > 0 && greyImage[x + 1, y + 1] + (int)((double)difference / 16) <= 255) greyImage[x + 1, y + 1] = greyImage[x + 1, y + 1] + (int)((double)difference / 16);
                        else greyImage[x + 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y + 1]);
                    }


                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    outputImage.SetPixel(x, y, Color.FromArgb(greyImage[x, y], greyImage[x, y], greyImage[x, y]));
                }
            }
            return outputImage;
        }

        private Bitmap Stucky(Bitmap outputImage)
        {
            List<int> greyPalette = new List<int>();
            int[,] greyImage = ConvertBitMapToGreyImage(outputImage);

            int step = 255 / (K - 1);
            for (int i = 0; i < K; i++)
            {
                greyPalette.Add(i * step);
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    int pixel = greyImage[x, y];
                    int newColor = FindMinimalGreyDistance(greyPalette, pixel);
                    int difference = pixel - newColor;
                    greyImage[x, y] = newColor;

                    if (x + 1 < imageWidth)
                    {
                        if (greyImage[x + 1, y] + (int)(8 * (double)difference / 42) > 0 && greyImage[x + 1, y] + (int)(8 * (double)difference / 42) <= 255) greyImage[x + 1, y] = greyImage[x + 1, y] + (int)(8 * (double)difference / 42);
                        else greyImage[x + 1, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y]); ;
                    }

                    if (x + 2 < imageWidth)
                    {
                        if (greyImage[x + 2, y] + (int)(4 * (double)difference / 42) > 0 && greyImage[x + 2, y] + (int)(4 * (double)difference / 42) <= 255) greyImage[x + 2, y] = greyImage[x + 2, y] + (int)(4 * (double)difference / 42);
                        else greyImage[x + 2, y] = FindMinimalGreyDistance(greyPalette, greyImage[x + 2, y]); ;
                    }

                    if (x - 1 > 0 && y + 1 < imageHeight)
                    {
                        if (greyImage[x - 1, y + 1] + (int)(4 * (double)difference / 42) > 0 && greyImage[x - 1, y + 1] + (int)(4 * (double)difference / 42) <= 255) greyImage[x - 1, y + 1] = greyImage[x - 1, y + 1] + (int)(4 * (double)difference / 42);
                        else greyImage[x - 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x - 1, y + 1]);
                    }

                    if (x - 2 > 0 && y + 1 < imageHeight)
                    {
                        if (greyImage[x - 2, y + 1] + (int)(2 * (double)difference / 42) > 0 && greyImage[x - 2, y + 1] + (int)(2 * (double)difference / 42) <= 255) greyImage[x - 2, y + 1] = greyImage[x - 2, y + 1] + (int)(2 * (double)difference / 42);
                        else greyImage[x - 2, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x - 2, y + 1]);
                    }

                    if (y + 1 < imageHeight)
                    {
                        if (greyImage[x, y + 1] + (int)(8 * (double)difference / 42) > 0 && greyImage[x, y + 1] + (int)(8 * (double)difference / 42) <= 255) greyImage[x, y + 1] = greyImage[x, y + 1] + (int)(8 * (double)difference / 42);
                        else greyImage[x, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x, y + 1]);
                    }

                    if (x + 1 < imageWidth && y + 1 < imageHeight)
                    {
                        if (greyImage[x + 1, y + 1] + (int)(4 * (double)difference / 42) > 0 && greyImage[x + 1, y + 1] + (int)(4 * (double)difference / 42) <= 255) greyImage[x + 1, y + 1] = greyImage[x + 1, y + 1] + (int)(4 * (double)difference / 42);
                        else greyImage[x + 1, y + 1] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y + 1]);
                    }

                    if (x + 2 < imageWidth && y + 2 < imageHeight)
                    {
                        if (greyImage[x + 2, y + 2] + (int)(1 * (double)difference / 42) > 0 && greyImage[x + 2, y + 2] + (int)(1 * (double)difference / 42) <= 255) greyImage[x + 2, y + 2] = greyImage[x + 2, y + 2] + (int)(1 * (double)difference / 42);
                        else greyImage[x + 2, y + 2] = FindMinimalGreyDistance(greyPalette, greyImage[x + 2, y + 2]);
                    }

                    if (x - 1 > 0 && y + 2 < imageHeight)
                    {
                        if (greyImage[x - 1, y + 2] + (int)(2 * (double)difference / 42) > 0 && greyImage[x - 1, y + 2] + (int)(2 * (double)difference / 42) <= 255) greyImage[x - 1, y + 2] = greyImage[x - 1, y + 2] + (int)(2 * (double)difference / 42);
                        else greyImage[x - 1, y + 2] = FindMinimalGreyDistance(greyPalette, greyImage[x - 1, y + 2]);
                    }

                    if (x - 2 > 0 && y + 2 < imageHeight)
                    {
                        if (greyImage[x - 2, y + 2] + (int)(1 * (double)difference / 42) > 0 && greyImage[x - 2, y + 2] + (int)(1 * (double)difference / 42) <= 255) greyImage[x - 2, y + 2] = greyImage[x - 2, y + 2] + (int)(1 * (double)difference / 42);
                        else greyImage[x - 2, y + 2] = FindMinimalGreyDistance(greyPalette, greyImage[x - 2, y + 2]);
                    }

                    if (y + 2 < imageHeight)
                    {
                        if (greyImage[x, y + 2] + (int)(4 * (double)difference / 42) > 0 && greyImage[x, y + 2] + (int)(4 * (double)difference / 42) <= 255) greyImage[x, y + 2] = greyImage[x, y + 2] + (int)(4 * (double)difference / 42);
                        else greyImage[x, y + 2] = FindMinimalGreyDistance(greyPalette, greyImage[x, y + 2]);
                    }

                    if (x + 1 < imageWidth && y + 2 < imageHeight)
                    {
                        if (greyImage[x + 1, y + 2] + (int)(2 * (double)difference / 42) > 0 && greyImage[x + 1, y + 2] + (int)(2 * (double)difference / 42) <= 255) greyImage[x + 1, y + 2] = greyImage[x + 1, y + 2] + (int)(2 * (double)difference / 42);
                        else greyImage[x + 1, y + 2] = FindMinimalGreyDistance(greyPalette, greyImage[x + 1, y + 2]);
                    }




                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    outputImage.SetPixel(x, y, Color.FromArgb(greyImage[x, y], greyImage[x, y], greyImage[x, y]));
                }
            }
            return outputImage;
        }

        private Bitmap ErrorDiffusion(Bitmap outputImage)
        {
            switch(matrixCombo.SelectedItem)
            {
                case Matrices.Atkinson:
                    outputImage = Atkinson(outputImage);
                    break;

                case Matrices.Burkes:
                    outputImage = Burkes(outputImage);
                    break;

                case Matrices.FloydSteinberg:
                    outputImage = FloydSteinberg(outputImage);
                    break;

                case Matrices.Sierra:
                    outputImage = Sierra(outputImage);
                    break;

                case Matrices.Stucky:
                    outputImage = Stucky(outputImage);
                    break;

                default:
                    outputImage = FloydSteinberg(outputImage);
                    break;
            }
            return outputImage;
        }

        private int[,,] MakeColorCube(RGB[,] image)
        {
            int[,,] cube = new int[256, 256, 256];

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    for (int k = 0; k < 256; k++)
                    {
                        cube[i, j, k] = 0;
                    }
                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {

                    cube[image[x,y].R, image[x, y].G, image[x, y].B]++;
                }
            }
            return cube;
        }

        private Vector[,,] MakeColorCube2(RGB[,] image)
        {
            Vector[,,] cube = new Vector[256, 256, 256];

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    for (int k = 0; k < 256; k++)
                    {
                        cube[i, j, k] =  new Vector(i, j, k, 0);
                        //cube[i, j, k].X = i;
                        //cube[i, j, k].Y = j;
                        //cube[i, j, k].Z = k;

                    }
                }
            }

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {

                    cube[image[x, y].R, image[x, y].G, image[x, y].B].Value++;
                }
            }
            return cube;
        }

        private void QuickSort(Vector[] array, int left, int right)
        {
            var i = left;
            var j = right;
            var pivot = array[(left + right) / 2].Value;
            while (i < j)
            {
                while (array[i].Value < pivot) i++;
                while (array[j].Value > pivot) j--;
                if (i <= j)
                {
                    // swap
                    Vector tmp = new Vector(array[i]);
                    array[i++] = new Vector(array[j]);  // ++ and -- inside array braces for shorter code
                    array[j--] = new Vector(tmp);
                }
            }
            if (left < j) QuickSort(array, left, j);
            if (i < right) QuickSort(array, i, right);
        }

        private bool CheckIfElementExists(Vector[] array, int k)
        {
            bool exists = false;

            for(int i = 0; i<K; i++)
            {
                if(array[i].Value == k)
                {
                    exists = true;
                    break;
                }
            }

            return exists;

        }

        private Vector[] FindMaximaOfCube2(Vector[,,] cube)
        {
            Vector[] maxima = new Vector[K];
            for(int i = 0; i<K; i++)
            {
                maxima[i] = new Vector(0, 0, 0, 0);
            }

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    for (int k = 0; k < 256; k++)
                    {
                        int value = cube[i, j, k].Value;

                        if (!CheckIfElementExists(maxima, value))
                        {
                            if (maxima[0].Value < value)
                            {
                                
                                maxima[0].X = cube[i, j, k].X;
                                maxima[0].Y = cube[i, j, k].Y;
                                maxima[0].Z = cube[i, j, k].Z;
                                maxima[0].Value = value;

                                QuickSort(maxima, 0, K - 1);
                            }
                        }

                    }
                }
            }

            return maxima;
        }

        private List<Vector> FindMaximaOfCube(int[,,] cube)
        {
           
            List<Vector> maxima = new List<Vector>();


            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    for (int k = 0; k < 256; k++)
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
                palette.Add(new RGB(vector.X, vector.Y, vector.Z));
            }

            return palette;
        }

        private RGB[] MakePalette2(Vector[] maxima)
        {
            RGB[] palette = new RGB[K];

            for(int i = 0; i<K; i++)
            {
                palette[i] = new RGB(maxima[i]);
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

        private RGB FindMinimalDistance(List<RGB> palette, RGB color)
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

        private RGB FindMinimalDistance2(RGB[] palette, RGB color)
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
            RGB[,] imageInRGB = ConvertBitmapToRGB(outputImage);
            //int[,,] cube = MakeColorCube(imageInRGB);
            //List<Vector> maxima = FindMaximaOfCube(cube);
            //List<RGB> palette = MakePalette(maxima);

            Vector[,,] cube = MakeColorCube2(imageInRGB);
            Vector[] maxima = FindMaximaOfCube2(cube);
            RGB[] palette = MakePalette2(maxima);

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    RGB newPixel = FindMinimalDistance2(palette, imageInRGB[x, y]);

                    outputImage.SetPixel(x, y, Color.FromArgb(newPixel.R, newPixel.G, newPixel.B));
                }
            }


            return outputImage;
        }      

        public List<Centroid> FindCentroids()
        {
            return new List<Centroid>();
        }

        public List<Centroid> RandomizeCentroids(RGB[,] pixels)
        {
            List<Centroid> centroids = new List<Centroid>();

            while(centroids.Count <K)
            {
                Random random = new Random();
                int x = random.Next(0, imageWidth);
                int y = random.Next(0, imageHeight);
                int R = pixels[x, y].R;
                int G = pixels[x, y].G;
                int B = pixels[x, y].B;
                bool flag = true;

                foreach (Centroid centroid in centroids)
                {
                    if(centroid.R == R && centroid.G == G && centroid.B == B)
                    {
                        flag = false;
                    }
                }
                if(flag)centroids.Add(new Centroid(R, G, B, new List<RGB>()));
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

        private void FindProperCentroid(List<Centroid> centroids, RGB color)
        {
            double min = 3 * 256 * 256;
            Centroid minimal = new Centroid(0, 0, 0, new List<RGB>());
            foreach (Centroid centroid in centroids)
            {
                double distance = DistanceBetweenColors(centroid.R, centroid.G, centroid.B, color.R, color.G, color.B);
                if (min > distance)
                {
                    min = distance;
                    minimal = centroid;
                }
            }

            minimal.pixels.Add(color);
        }

        public void AssignPixlesToCentroid(List<Centroid> centroids, RGB[,] image)
        {
            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    FindProperCentroid(centroids, image[x, y]);
                }
            }            
        }

        private void FindeNewCenterOfGravity(List<Centroid> centroids, int counter)
        {
            foreach(Centroid centroid in centroids)
            {
                int sumR = 0;
                int sumG = 0;
                int sumB = 0;
                foreach (RGB pixel in centroid.pixels)
                {
                    sumR += pixel.R;
                    sumG += pixel.G;
                    sumB += pixel.B;
                }

                int count = centroid.pixels.Count;
                sumR = sumR / count;
                sumG = sumG / count;
                sumB = sumB / count;

                if (centroid.R != sumR && centroid.G != sumG && centroid.B != sumB)
                {

                        centroid.R = sumR;
                        centroid.G = sumG;
                        centroid.B = sumB;
                        counter++;

                }
            }
        }

        public void IterateThroughCentroids(List<Centroid> centroids, RGB[,] pixels)
        {
            int counter;
            do
            {
                counter = 0;
                AssignPixlesToCentroid(centroids, pixels);
                FindeNewCenterOfGravity(centroids, counter);
            }
            while (counter != 0);
        }

        private List<RGB> MakePalette(List<Centroid> centroids)
        {
            List<RGB> palette = new List<RGB>();

            foreach (Centroid centroid in centroids)
            {
                palette.Add(new RGB(centroid.R, centroid.G, centroid.B));
            }

            return palette;
        }

        private Bitmap kMeansColorQuantization(Bitmap outputImage)
        {
            RGB[,] imageInPixles = ConvertBitmapToRGB(outputImage);
            List<Centroid> centroids = RandomizeCentroids(imageInPixles);
            IterateThroughCentroids(centroids, imageInPixles);
            List<RGB> palette = MakePalette(centroids);

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {

                    RGB newPixel = FindMinimalDistance(palette, imageInPixles[x,y]);

                    outputImage.SetPixel(x, y, Color.FromArgb(newPixel.R, newPixel.G, newPixel.B));
                }
            }



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

            matrixCombo.Items.Add(Matrices.FloydSteinberg);
            matrixCombo.Items.Add(Matrices.Atkinson);
            matrixCombo.Items.Add(Matrices.Burkes);
            matrixCombo.Items.Add(Matrices.Sierra);
            matrixCombo.Items.Add(Matrices.Stucky);
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
