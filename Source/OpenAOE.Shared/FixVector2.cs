using System;

namespace OpenAOE
{
    public struct FixVector2
    {
        public double X;
        public double Y;

        public double Magnitude => Math.Sqrt(X*X+Y*Y);

        public FixVector2 Normalized => this/Magnitude;

        public FixVector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        #region Operators

        public static FixVector2 operator *(FixVector2 value, double scaleFactor)
        {
            FixVector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }

        public static FixVector2 operator *(double scaleFactor, FixVector2 value)
        {
            FixVector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }

        public static FixVector2 operator /(FixVector2 value, double scaleFactor)
        {
            FixVector2 vector;
            vector.X = value.X / scaleFactor;
            vector.Y = value.Y / scaleFactor;
            return vector;
        }

        public static FixVector2 operator /(double scaleFactor, FixVector2 value)
        {
            FixVector2 vector;
            vector.X = value.X / scaleFactor;
            vector.Y = value.Y / scaleFactor;
            return vector;
        }

        public static FixVector2 operator +(FixVector2 value1, FixVector2 value2)
        {
            FixVector2 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }

        public static FixVector2 operator -(FixVector2 point1, FixVector2 point2)
        {
            return new FixVector2(point1.X - point2.X, point1.Y - point2.Y);
        }

        #endregion
    }
}
