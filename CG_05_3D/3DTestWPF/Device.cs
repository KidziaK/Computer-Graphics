using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace _3DTestWPF
{
    public class Device
    {
        #region Properties
        private byte[] backBuffer;
        private readonly float[] depthBuffer;
        private object[] lockBuffer;
        private WriteableBitmap bmp;
        private readonly int renderWidth;
        private readonly int renderHeight;
        #endregion

        public Device(WriteableBitmap bmp)
        {
            this.bmp = bmp;
            renderWidth = bmp.PixelWidth;
            renderHeight = bmp.PixelHeight;

            // the back buffer size is equal to the number of pixels to draw
            // on screen (width*height) * 4 (R,G,B & Alpha values). 
            backBuffer = new byte[bmp.PixelWidth * bmp.PixelHeight * 4];
            depthBuffer = new float[bmp.PixelWidth * bmp.PixelHeight];
            lockBuffer = new object[renderWidth * renderHeight];
            for (var i = 0; i < lockBuffer.Length; i++)
            {
                lockBuffer[i] = new object();
            }
        }

        // This method is called to clear the back buffer with a specific color
        public void Clear(byte r, byte g, byte b, byte a) {
            for (var index = 0; index < backBuffer.Length; index += 4)
            {
                // BGRA is used by Windows instead by RGBA in HTML5
                backBuffer[index] = b;
                backBuffer[index + 1] = g;
                backBuffer[index + 2] = r;
                backBuffer[index + 3] = a;
            }

            // Clearing Depth Buffer
            for (var index = 0; index < depthBuffer.Length; index++)
            {
                depthBuffer[index] = float.MaxValue;
            }
        }

        // Once everything is ready, we can flush the back buffer
        // into the front buffer. 
        public void Present()
        {
            bmp.Lock();
            using (var ms = new MemoryStream())
            {
                // writing our byte[] back buffer into our WriteableBitmap stream
                ms.Write(backBuffer, 0, backBuffer.Length);
            }

            bmp.WritePixels( new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight), backBuffer, bmp.PixelWidth*4, 0);

            // request a redraw of the entire bitmap
            bmp.AddDirtyRect(new Int32Rect(0, 0, (int)bmp.Width, (int)bmp.Height));
            bmp.Unlock();
        }

        // Called to put a pixel on screen at a specific X,Y coordinates
        public void PutPixel(int x, int y, float z, Color4 color)
        {
            // As we have a 1-D Array for our back buffer
            // we need to know the equivalent cell in 1-D based
            // on the 2D coordinates on screen
            var index = (x + y * renderWidth);
            var index4 = index * 4;

            if (depthBuffer[index] < z)
            {
                return; // Discard
            }

            depthBuffer[index] = z;

            backBuffer[index4] = (byte)(color.Blue * 255);
            backBuffer[index4 + 1] = (byte)(color.Green * 255);
            backBuffer[index4 + 2] = (byte)(color.Red * 255);
            backBuffer[index4 + 3] = (byte)(color.Alpha * 255);
        }

        //// Project takes some 3D coordinates and transform them
        //// in 2D coordinates using the transformation matrix
        //public Vector3 Project(Vector3 coord, Matrix transMat)
        //{
        //    // transforming the coordinates
        //    var point = Vector3.TransformCoordinate(coord, transMat);
        //    // The transformed coordinates will be based on coordinate system
        //    // starting on the center of the screen. But drawing on screen normally starts
        //    // from top left. We then need to transform them again to have x:0, y:0 on top left.
        //    var x = point.X * bmp.PixelWidth + bmp.PixelWidth / 2.0f;
        //    var y = -point.Y * bmp.PixelHeight + bmp.PixelHeight / 2.0f;
        //    return (new Vector3(x, y, point.Z));
        //}

        // Project takes some 3D coordinates and transform them
        // in 2D coordinates using the transformation matrix
        public Vector3 Project(Vector3 coord, Matrix transMat)
        {
            // transforming the coordinates
            var point = Vector3.TransformCoordinate(coord, transMat);
            // The transformed coordinates will be based on coordinate system
            // starting on the center of the screen. But drawing on screen normally starts
            // from top left. We then need to transform them again to have x:0, y:0 on top left.
            var x = point.X * bmp.PixelWidth + bmp.PixelWidth / 2.0f;
            var y = -point.Y * bmp.PixelHeight + bmp.PixelHeight / 2.0f;
            return (new Vector3(x, y, point.Z));
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        void ProcessScanLine(int y, Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pd, Color4 color)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            // starting Z & ending Z
            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);

            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Interpolate(z1, z2, gradient);
                DrawPoint(new Vector3(x, y, z), color);
            }
        }

        // Compute the cosine of the angle between the light vector and the normal vector
        // Returns a value between 0 and 1
        float ComputeNDotL(Vector3 vertex, Vector3 normal, Vector3 lightPosition)
        {
            var lightDirection = lightPosition - vertex;

            normal.Normalize();
            lightDirection.Normalize();

            return Math.Max(0, Vector3.Dot(normal, lightDirection));
        }

        // DrawPoint calls PutPixel but does the clipping operation before
        public void DrawPoint(Vector3 point, Color4 color)
        {
            // Clipping what's visible on screen
            if (point.X >= 0 && point.Y >= 0 && point.X < bmp.PixelWidth && point.Y < bmp.PixelHeight)
            {
                // Drawing a point
                PutPixel((int)point.X, (int)point.Y, point.Z, color);
            }
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        //void ProcessScanLine(int y, Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pd, Color4 color)
        //{
        //    // Thanks to current Y, we can compute the gradient to compute others values like
        //    // the starting X (sx) and ending X (ex) to draw between
        //    // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
        //    var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
        //    var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

        //    int sx = (int)Interpolate(pa.X, pb.X, gradient1);
        //    int ex = (int)Interpolate(pc.X, pd.X, gradient2);

        //    // starting Z & ending Z
        //    float z1 = Interpolate(pa.Z, pb.Z, gradient1);
        //    float z2 = Interpolate(pc.Z, pd.Z, gradient2);

        //    // drawing a line from left (sx) to right (ex) 
        //    for (var x = sx; x < ex; x++)
        //    {
        //        float gradient = (x - sx) / (float)(ex - sx);

        //        var z = Interpolate(z1, z2, gradient);
        //        DrawPoint(new Vector3(x, y, z), color);
        //    }
        //}

        //public void DrawBline(Vector2 point0, Vector2 point1)
        //{
        //    int x0 = (int)point0.X;
        //    int y0 = (int)point0.Y;
        //    int x1 = (int)point1.X;
        //    int y1 = (int)point1.Y;

        //    var dx = Math.Abs(x1 - x0);
        //    var dy = Math.Abs(y1 - y0);
        //    var sx = (x0 < x1) ? 1 : -1;
        //    var sy = (y0 < y1) ? 1 : -1;
        //    var err = dx - dy;

        //    while (true)
        //    {
        //        DrawPoint(new Vector2(x0, y0));

        //        if ((x0 == x1) && (y0 == y1)) break;
        //        var e2 = 2 * err;
        //        if (e2 > -dy) { err -= dy; x0 += sx; }
        //        if (e2 < dx) { err += dx; y0 += sy; }
        //    }
        //}

        //public void DrawTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Color4 color)
        //{
        //    // Sorting the points in order to always have this order on screen p1, p2 & p3
        //    // with p1 always up (thus having the Y the lowest possible to be near the top screen)
        //    // then p2 between p1 & p3
        //    if (p1.Y > p2.Y)
        //    {
        //        var temp = p2;
        //        p2 = p1;
        //        p1 = temp;
        //    }

        //    if (p2.Y > p3.Y)
        //    {
        //        var temp = p2;
        //        p2 = p3;
        //        p3 = temp;
        //    }

        //    if (p1.Y > p2.Y)
        //    {
        //        var temp = p2;
        //        p2 = p1;
        //        p1 = temp;
        //    }

        //    // computing lines' directions
        //    float dP1P2, dP1P3;


        //    // Computing slopes
        //    if (p2.Y - p1.Y > 0)
        //        dP1P2 = (p2.X - p1.X) / (p2.Y - p1.Y);
        //    else
        //        dP1P2 = 0;

        //    if (p3.Y - p1.Y > 0)
        //        dP1P3 = (p3.X - p1.X) / (p3.Y - p1.Y);
        //    else
        //        dP1P3 = 0;

        //    // First case where triangles are like that:
        //    // P1
        //    // -
        //    // -- 
        //    // - -
        //    // -  -
        //    // -   - P2
        //    // -  -
        //    // - -
        //    // -
        //    // P3
        //    if (dP1P2 > dP1P3)
        //    {
        //        for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
        //        {
        //            if (y < p2.Y)
        //            {
        //                ProcessScanLine(y, p1, p3, p1, p2, color);
        //            }
        //            else
        //            {
        //                ProcessScanLine(y, p1, p3, p2, p3, color);
        //            }
        //        }
        //    }
        //    // First case where triangles are like that:
        //    //       P1
        //    //        -
        //    //       -- 
        //    //      - -
        //    //     -  -
        //    // P2 -   - 
        //    //     -  -
        //    //      - -
        //    //        -
        //    //       P3
        //    else
        //    {
        //        for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
        //        {
        //            if (y < p2.Y)
        //            {
        //                ProcessScanLine(y, p1, p2, p1, p3, color);
        //            }
        //            else
        //            {
        //                ProcessScanLine(y, p2, p3, p1, p3, color);
        //            }
        //        }
        //    }
        //}

        // Clamping values to keep them between 0 and 1

        public void DrawTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Color4 color)
        {
            // Sorting the points in order to always have this order on screen p1, p2 & p3
            // with p1 always up (thus having the Y the lowest possible to be near the top screen)
            // then p2 between p1 & p3
            if (p1.Y > p2.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            if (p2.Y > p3.Y)
            {
                var temp = p2;
                p2 = p3;
                p3 = temp;
            }

            if (p1.Y > p2.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            // computing lines' directions
            float dP1P2, dP1P3;

            // Computing slopes
            if (p2.Y - p1.Y > 0)
                dP1P2 = (p2.X - p1.X) / (p2.Y - p1.Y);
            else
                dP1P2 = 0;

            if (p3.Y - p1.Y > 0)
                dP1P3 = (p3.X - p1.X) / (p3.Y - p1.Y);
            else
                dP1P3 = 0;

            // First case where triangles are like that:
            // P1
            // -
            // -- 
            // - -
            // -  -
            // -   - P2
            // -  -
            // - -
            // -
            // P3
            if (dP1P2 > dP1P3)
            {
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                    {
                        ProcessScanLine(y, p1, p3, p1, p2, color);
                    }
                    else
                    {
                        ProcessScanLine(y, p1, p3, p2, p3, color);
                    }
                }
            }
            // First case where triangles are like that:
            //       P1
            //        -
            //       -- 
            //      - -
            //     -  -
            // P2 -   - 
            //     -  -
            //      - -
            //        -
            //       P3
            else
            {
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                    {
                        ProcessScanLine(y, p1, p2, p1, p3, color);
                    }
                    else
                    {
                        ProcessScanLine(y, p2, p3, p1, p3, color);
                    }
                }
            }
        }
        float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        // Interpolating the value between 2 vertices 
        // min is the starting point, max the ending point
        // and gradient the % between the 2 points
        float Interpolate(float min, float max, float gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }

        // The main method of the engine that re-compute each vertex projection
        // during each frame
        public void Render(Camera camera, params Mesh[] meshes)
        {
            // To understand this part, please read the prerequisites resources
            var viewMatrix = Matrix.LookAtLH(camera.Position, camera.Target, new Vector3(0, 1, 0));
            var projectionMatrix = Matrix.PerspectiveFovLH(0.78f,
                                                           (float)bmp.PixelWidth / bmp.PixelHeight,
                                                           0.01f, 1.0f);

            foreach (Mesh mesh in meshes)
            {
                // Beware to apply rotation before translation 
                var worldMatrix = Matrix.RotationYawPitchRoll(mesh.Rotation.Y, mesh.Rotation.X, mesh.Rotation.Z) *
                                  Matrix.Translation(mesh.Position);

                var transformMatrix = worldMatrix * viewMatrix * projectionMatrix;

                var faceIndex = 0;
                foreach (var face in mesh.Faces)
                {
                    var vertexA = mesh.Vertices[face.A];
                    var vertexB = mesh.Vertices[face.B];
                    var vertexC = mesh.Vertices[face.C];

                    var pixelA = Project(vertexA, transformMatrix);
                    var pixelB = Project(vertexB, transformMatrix);
                    var pixelC = Project(vertexC, transformMatrix);

                    var color = 0.25f + (faceIndex % mesh.Faces.Length) * 0.75f / mesh.Faces.Length;
                    DrawTriangle(pixelA, pixelB, pixelC, new Color4(color, color, color, 1));
                    faceIndex++;
                }
            }
        }
    }
}
