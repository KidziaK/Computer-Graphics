using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DTestWPF
{
    public struct Matrix
    {
        public static readonly Matrix Identity = new Matrix() { M11 = 1.0f, M22 = 1.0f, M33 = 1.0f, M44 = 1.0f };

        public float M11;

        public float M12;

        public float M13;

        public float M14;

        public float M21;

        public float M22;

        public float M23;

        public float M24;

        public float M31;

        public float M32;

        public float M33;

        public float M34;

        public float M41;

        public float M42;

        public float M43;

        public float M44;

        public Matrix(float value)
        {
            M11 = M12 = M13 = M14 =
            M21 = M22 = M23 = M24 =
            M31 = M32 = M33 = M34 =
            M41 = M42 = M43 = M44 = value;
        }

        public Matrix(float M11, float M12, float M13, float M14, float M21, float M22, float M23, float M24, float M31, float M32, float M33, float M34, float M41, float M42, float M43, float M44)
        {
            this.M11 = M11; this.M12 = M12; this.M13 = M13; this.M14 = M14;
            this.M21 = M21; this.M22 = M22; this.M23 = M23; this.M24 = M24;
            this.M31 = M31; this.M32 = M32; this.M33 = M33; this.M34 = M34;
            this.M41 = M41; this.M42 = M42; this.M43 = M43; this.M44 = M44;
        }

        public static void Multiply(ref Matrix left, float right, out Matrix result)
        {
            result.M11 = left.M11 * right;
            result.M12 = left.M12 * right;
            result.M13 = left.M13 * right;
            result.M14 = left.M14 * right;
            result.M21 = left.M21 * right;
            result.M22 = left.M22 * right;
            result.M23 = left.M23 * right;
            result.M24 = left.M24 * right;
            result.M31 = left.M31 * right;
            result.M32 = left.M32 * right;
            result.M33 = left.M33 * right;
            result.M34 = left.M34 * right;
            result.M41 = left.M41 * right;
            result.M42 = left.M42 * right;
            result.M43 = left.M43 * right;
            result.M44 = left.M44 * right;
        }

        public static Matrix Multiply(Matrix left, float right)
        {
            Matrix result;
            Multiply(ref left, right, out result);
            return result;
        }

        public static void Multiply(ref Matrix left, ref Matrix right, out Matrix result)
        {
            result = new Matrix();
            result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41);
            result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42);
            result.M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43);
            result.M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44);
            result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41);
            result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42);
            result.M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43);
            result.M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44);
            result.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41);
            result.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42);
            result.M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43);
            result.M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44);
            result.M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41);
            result.M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42);
            result.M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43);
            result.M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44);
        }

        public static Matrix Multiply(Matrix left, Matrix right)
        {
            Matrix result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        public static Matrix operator *(float left, Matrix right)
        {
            Matrix result;
            Multiply(ref right, left, out result);
            return result;
        }
        public static Matrix operator *(Matrix left, float right)
        {
            Matrix result;
            Multiply(ref left, right, out result);
            return result;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            Matrix result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        public static void LookAtLH(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix result)
        {
            Vector3 xaxis, yaxis, zaxis;
            Vector3.Subtract(ref target, ref eye, out zaxis); zaxis.Normalize();
            Vector3.Cross(ref up, ref zaxis, out xaxis); xaxis.Normalize();
            Vector3.Cross(ref zaxis, ref xaxis, out yaxis);

            result = Matrix.Identity;
            result.M11 = xaxis.X; result.M21 = xaxis.Y; result.M31 = xaxis.Z;
            result.M12 = yaxis.X; result.M22 = yaxis.Y; result.M32 = yaxis.Z;
            result.M13 = zaxis.X; result.M23 = zaxis.Y; result.M33 = zaxis.Z;

            Vector3.Dot(ref xaxis, ref eye, out result.M41);
            Vector3.Dot(ref yaxis, ref eye, out result.M42);
            Vector3.Dot(ref zaxis, ref eye, out result.M43);

            result.M41 = -result.M41;
            result.M42 = -result.M42;
            result.M43 = -result.M43;
        }

        public static Matrix LookAtLH(Vector3 eye, Vector3 target, Vector3 up)
        {
            Matrix result;
            LookAtLH(ref eye, ref target, ref up, out result);
            return result;
        }

        public static void PerspectiveFovRH(float fov, float aspect, float znear, float zfar, out Matrix result)
        {
            float yScale = (float)(1.0 / Math.Tan(fov * 0.5f));
            float xScale = yScale / aspect;

            float halfWidth = znear / xScale;
            float halfHeight = znear / yScale;

            PerspectiveOffCenterRH(-halfWidth, halfWidth, -halfHeight, halfHeight, znear, zfar, out result);
        }
        public static Matrix PerspectiveFovRH(float fov, float aspect, float znear, float zfar)
        {
            Matrix result;
            PerspectiveFovRH(fov, aspect, znear, zfar, out result);
            return result;
        }

        public static void PerspectiveOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            float zRange = zfar / (zfar - znear);

            result = new Matrix();
            result.M11 = 2.0f * znear / (right - left);
            result.M22 = 2.0f * znear / (top - bottom);
            result.M31 = (left + right) / (left - right);
            result.M32 = (top + bottom) / (bottom - top);
            result.M33 = zRange;
            result.M34 = 1.0f;
            result.M43 = -znear * zRange;
        }
        public static Matrix PerspectiveOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result;
            PerspectiveOffCenterLH(left, right, bottom, top, znear, zfar, out result);
            return result;
        }
        public static void PerspectiveOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            PerspectiveOffCenterLH(left, right, bottom, top, znear, zfar, out result);
            result.M31 *= -1.0f;
            result.M32 *= -1.0f;
            result.M33 *= -1.0f;
            result.M34 *= -1.0f;
        }
        public static Matrix PerspectiveOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result;
            PerspectiveOffCenterRH(left, right, bottom, top, znear, zfar, out result);
            return result;
        }

        public static void RotationYawPitchRoll(float yaw, float pitch, float roll, out Matrix result)
        {
            Quaternion quaternion = new Quaternion();
            Quaternion.RotationYawPitchRoll(yaw, pitch, roll, out quaternion);
            RotationQuaternion(ref quaternion, out result);
        }
        public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Matrix result;
            RotationYawPitchRoll(yaw, pitch, roll, out result);
            return result;
        }

        public static void RotationQuaternion(ref Quaternion rotation, out Matrix result)
        {
            float xx = rotation.X * rotation.X;
            float yy = rotation.Y * rotation.Y;
            float zz = rotation.Z * rotation.Z;
            float xy = rotation.X * rotation.Y;
            float zw = rotation.Z * rotation.W;
            float zx = rotation.Z * rotation.X;
            float yw = rotation.Y * rotation.W;
            float yz = rotation.Y * rotation.Z;
            float xw = rotation.X * rotation.W;

            result = Matrix.Identity;
            result.M11 = 1.0f - (2.0f * (yy + zz));
            result.M12 = 2.0f * (xy + zw);
            result.M13 = 2.0f * (zx - yw);
            result.M21 = 2.0f * (xy - zw);
            result.M22 = 1.0f - (2.0f * (zz + xx));
            result.M23 = 2.0f * (yz + xw);
            result.M31 = 2.0f * (zx + yw);
            result.M32 = 2.0f * (yz - xw);
            result.M33 = 1.0f - (2.0f * (yy + xx));
        }
        public static Matrix RotationQuaternion(Quaternion rotation)
        {
            Matrix result;
            RotationQuaternion(ref rotation, out result);
            return result;
        }

        public static void Translation(ref Vector3 value, out Matrix result)
        {
            Translation(value.X, value.Y, value.Z, out result);
        }
        public static Matrix Translation(Vector3 value)
        {
            Matrix result;
            Translation(ref value, out result);
            return result;
        }
        public static void Translation(float x, float y, float z, out Matrix result)
        {
            result = Matrix.Identity;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
        }

        public static Matrix Translation(float x, float y, float z)
        {
            Matrix result;
            Translation(x, y, z, out result);
            return result;
        }

        public static void PerspectiveFovLH(float fov, float aspect, float znear, float zfar, out Matrix result)
        {
            float yScale = (float)(1.0 / Math.Tan(fov * 0.5f));
            float xScale = yScale / aspect;

            float halfWidth = znear / xScale;
            float halfHeight = znear / yScale;

            PerspectiveOffCenterLH(-halfWidth, halfWidth, -halfHeight, halfHeight, znear, zfar, out result);
        }

        /// <summary>
        /// Creates a left-handed, perspective projection matrix based on a field of view.
        /// </summary>
        /// <param name="fov">Field of view in the y direction, in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <returns>The created projection matrix.</returns>
        public static Matrix PerspectiveFovLH(float fov, float aspect, float znear, float zfar)
        {
            Matrix result;
            PerspectiveFovLH(fov, aspect, znear, zfar, out result);
            return result;
        }



    }
}
