using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _3DTestWPF
{
    public class Texture
    {
        private byte[] internalBuffer;
        private int width;
        private int height;

        // Working with a fix sized texture (512x512, 1024x1024, etc.).
        public Texture(string filename, int width, int height)
        {
            this.width = width;
            this.height = height;
            Load(filename);
        }

        public void Load(string filename)
        {
            BitmapImage bitmap = new BitmapImage(new Uri("pattern.jpg", UriKind.Relative));
            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmap);
            bitmap.CopyPixels(internalBuffer, width * 4, 0);
        }

        // Takes the U & V coordinates exported by Blender
        // and return the corresponding pixel color in the texture
        public Color4 Map(float tu, float tv)
        {
            // Image is not loaded yet
            if (internalBuffer == null)
            {
                return new Color4();
            }
            // using a % operator to cycle/repeat the texture if needed
            int u = Math.Abs((int)(tu * width) % width);
            int v = Math.Abs((int)(tv * height) % height);

            int pos = (u + v * width) * 4;
            byte b = internalBuffer[pos];
            byte g = internalBuffer[pos + 1];
            byte r = internalBuffer[pos + 2];
            byte a = internalBuffer[pos + 3];

            return new Color4(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }
    }
}
