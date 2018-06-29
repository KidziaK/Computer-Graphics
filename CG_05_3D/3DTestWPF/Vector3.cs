using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DTestWPF
{
    public struct Vector3
    {
        public const float ZeroTolerance = 1e-6f;

        public float X;

        public float Y;

        public float Z;

        public Vector3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 value, float z)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
        }

        public Vector3(Vector3 vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }

        public float Length()
        {
            return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = new Vector3((left.Y * right.Z) - (left.Z * right.Y), (left.Z * right.X) - (left.X * right.Z), (left.X * right.Y) - (left.Y * right.X));
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
        {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        public static void Subtract(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3 Subtract(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            result = value;
            result.Normalize();
        }

        public static Vector3 Normalize(Vector3 value)
        {
            value.Normalize();
            return value;
        }

        public void Normalize()
        {
            float length = Length();
            if (length > ZeroTolerance)
            {
                float inv = 1.0f / length;
                X *= inv;
                Y *= inv;
                Z *= inv;
            }
        }

        public static void TransformCoordinate(ref Vector3 coordinate, ref Matrix transform, out Vector3 result)
        {
            Vector4 vector = new Vector4();
            vector.X = (coordinate.X * transform.M11) + (coordinate.Y * transform.M21) + (coordinate.Z * transform.M31) + transform.M41;
            vector.Y = (coordinate.X * transform.M12) + (coordinate.Y * transform.M22) + (coordinate.Z * transform.M32) + transform.M42;
            vector.Z = (coordinate.X * transform.M13) + (coordinate.Y * transform.M23) + (coordinate.Z * transform.M33) + transform.M43;
            vector.W = 1f / ((coordinate.X * transform.M14) + (coordinate.Y * transform.M24) + (coordinate.Z * transform.M34) + transform.M44);

            result = new Vector3(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W);
        }

        public static Vector3 TransformCoordinate(Vector3 coordinate, Matrix transform)
        {
            Vector3 result;
            TransformCoordinate(ref coordinate, ref transform, out result);
            return result;
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3 operator +(Vector3 value)
        {
            return value;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3 operator -(Vector3 value)
        {
            return new Vector3(-value.X, -value.Y, -value.Z);
        }


    }
}
