using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DTestWPF
{
    public struct Vector2
    {
        public float X;

        public float Y;

        public Vector2(float value)
        {
            X = value;
            Y = value;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }
        public static Vector2 operator +(Vector2 value)
        {
            return value;
        }
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }
        public static Vector2 operator -(Vector2 value)
        {
            return new Vector2(-value.X, -value.Y);
        }

        public float Length()
        {
            return (float)Math.Sqrt((X * X) + (Y * Y));
        }

        public static Vector2 operator /(Vector2 value, float scale)
        {
            return new Vector2(value.X / scale, value.Y / scale);
        }
    }
}
