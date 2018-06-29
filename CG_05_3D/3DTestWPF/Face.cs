using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DTestWPF
{
    public struct Face
    {
        public int A;
        public int B;
        public int C;

        public Vector3 Normal;

        public Face(Face face)
        {
            A = face.A;
            B = face.B;
            C = face.C;

            Normal = new Vector3(face.Normal);
        }
   
    }
}
