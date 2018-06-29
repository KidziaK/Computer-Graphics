using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;

namespace _3DTestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int n = 10;
        static float h = 1.5f;
        static float r = 0.3f;
        private Device device;
        Mesh mesh = new Mesh("Cylinder", 4*n + 2, 4*n);
        Camera camera = new Camera();


        public MainWindow()
        {           
            InitializeComponent();

            WriteableBitmap bmp = new WriteableBitmap(1000, 600, 96, 96, PixelFormats.Bgr32, BitmapPalettes.Halftone256);

            device = new Device(bmp);

            frontBuffer.Source = bmp;

            for(int i = 0; i <= n; i++)
            {
                mesh.Vertices[i].Normal = new Vector3(0, 1, 0);
            }

            mesh.Vertices[0].Coordinates = new Vector3(0, h, 0);
            for(int i = 1; i<=n; i++)
            {
                mesh.Vertices[i].Coordinates = new Vector3((float)(r * Math.Cos(2 * Math.PI / n * (i - 1))), h, (float)(r * Math.Sin(2 * Math.PI / n * (i - 1))));
            }

            for (int i = 0; i <= n-1; i++)
            {
                mesh.Faces[i] = new Face { A = 0, B = (i + 2) % (n + 1), C = i + 1 };
            }

            mesh.Vertices[4 * n + 1].Coordinates = new Vector3(0, 0, 0);


            for (int i = 3 * n + 1; i <= 4 * n; i++)
            {
                mesh.Vertices[i].Normal = new Vector3(0, -1, 0);
                mesh.Vertices[i].Coordinates = new Vector3((float)(r * Math.Cos(2 * Math.PI / n * (i - 1))), 0, (float)(r * Math.Sin(2 * Math.PI / n * (i - 1))));
            }

            for (int i = 3 * n; i <= 4 * n - 2; i++)
            {
                mesh.Faces[i] = new Face { A = 4 * n + 1, B = i + 1, C = i + 2 };
            }

            mesh.Faces[4 * n - 1] = new Face { A = 4 * n + 1, B = 4 * n, C = 3 * n + 1 };

            for (int i = n + 1; i <= 2 * n; i++)
            {
                mesh.Vertices[i].Coordinates = new Vector3(mesh.Vertices[i - n].Coordinates);
            }

            for (int i = 2*n + 1; i <= 3 * n; i++)
            {
                mesh.Vertices[i].Coordinates = new Vector3(mesh.Vertices[i + n].Coordinates);
            }

            for(int i = n + 1; i <= 3 * n; i++)
            {
                mesh.Vertices[i].Normal = new Vector3(mesh.Vertices[i].Coordinates.X / r, 0, mesh.Vertices[i].Coordinates.Z / r);
            }

            for (int i = n; i <= 2 * n - 2; i++)
            {
                mesh.Faces[i] = new Face { A = i + 1, B = i + 2, C = i + 1 + n };
            }

            mesh.Faces[2*n - 1] = new Face { A = 2 * n, B = n + 1, C = 3 * n };

            for (int i = 2 * n; i <= 3 * n - 2; i++)
            {
                mesh.Faces[i] = new Face { A = i + 1, B = i + 2 - n, C = i + 2 };
            }

            mesh.Faces[3 * n - 1] = new Face { A = 3 * n, B = n + 1, C = 2 * n + 1 };



            for(int i = 0; i <= 4*n + 1; i++)
            {
              

                mesh.Vertices[i].TextureCoordinates = new Vector2(294 / n * i, 171 / n * i);
            }

            camera.Position = new Vector3(0, 0, 10.0f);
            camera.Target = new Vector3(0, 0, 0);

            mesh.Texture = new Texture("pattern", 294, 171);

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            device.Clear(0, 0, 0, 255);

            // rotating slightly the cube during each frame rendered
            mesh.Rotation = new Vector3(mesh.Rotation.X + 0.01f, mesh.Rotation.Y + 0.01f, mesh.Rotation.Z);
            // Doing the various matrix operations
            device.Render(camera, mesh);
            // Flushing the back buffer into the front buffer
            device.Present();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch(e.Key)
            {
                case System.Windows.Input.Key.Up:
                    lock(camera)
                    {
                        camera.Target += new Vector3(0, 0.1f, 0);
                    }
                    break;

                case System.Windows.Input.Key.Down:
                    lock (camera)
                    {                       
                        camera.Target += new Vector3(0, -0.1f, 0);
                    }
                    break;

                case System.Windows.Input.Key.Left:
                    lock (camera)
                    {
                        camera.Target += new Vector3(0.1f, 0, 0);
                    }
                    break;

                case System.Windows.Input.Key.Right:
                    lock (camera)
                    {
                        camera.Target += new Vector3(-0.1f, 0, 0);
                    }
                    break;

                case System.Windows.Input.Key.A:
                    lock (camera)
                    {
                        camera.Position += new Vector3(0.1f, 0, -0.1f);
                    }
                    break;

                case System.Windows.Input.Key.W:
                    lock (camera)
                    {
                        camera.Position += new Vector3(0, -0.1f, 0.1f);
                    }
                    break;

                case System.Windows.Input.Key.S:
                    lock (camera)
                    {
                        camera.Position += new Vector3(0, 0.1f, -0.1f);
                    }
                    break;

                case System.Windows.Input.Key.D:
                    lock (camera)
                    {
                        camera.Position += new Vector3(-0.1f, 0, 0.1f);
                    }
                    break;
            }
        }
    }
}
