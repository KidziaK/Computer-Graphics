﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DTestWPF
{
    public struct Vector4
    {
        public float X;

        public float Y;

        public float Z;

        public float W;


        public Vector4(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}
