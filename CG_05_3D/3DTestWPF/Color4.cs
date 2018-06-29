namespace _3DTestWPF
{
    public struct Color4
    {
        public static readonly Color4 White = new Color4(1.0f, 1.0f, 1.0f, 1.0f);

        public float Red;

        public float Green;

        public float Blue;

        public float Alpha;

        public Color4(float value)
        {
            Alpha = Red = Green = Blue = value;
        }

        public Color4(float red, float green, float blue, float alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public Color4(Vector4 value)
        {
            Red = value.X;
            Green = value.Y;
            Blue = value.Z;
            Alpha = value.W;
        }

        public static Color4 operator *(float scale, Color4 value)
        {
            return new Color4(value.Red * scale, value.Green * scale, value.Blue * scale, value.Alpha * scale);
        }
        public static Color4 operator *(Color4 value, float scale)
        {
            return new Color4(value.Red * scale, value.Green * scale, value.Blue * scale, value.Alpha * scale);
        }
        public static Color4 operator *(Color4 left, Color4 right)
        {
            return new Color4(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue, left.Alpha * right.Alpha);
        }
    }
}
