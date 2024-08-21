using System.Runtime.InteropServices;

namespace McProtoNet;

[StructLayout(LayoutKind.Explicit)]
public struct Vector3 : IEquatable<Vector3>
{
    [FieldOffset(0)] public double X;
    [FieldOffset(8)] public double Y;
    [FieldOffset(16)] public double Z;

    public Vector3(double value)
    {
        X = Y = Z = value;
    }

    public Vector3(Vector3 from, Vector3 to)
    {
        X = to.X - from.X;
        Y = to.Y - from.Y;
        Z = to.Z - from.Z;
    }

    public Vector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(Vector3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    public override string ToString()
    {
        return string.Format("<{0},{1},{2}>", X, Y, Z);
    }


    public static bool operator !=(Vector3 a, Vector3 b)
    {
        return !a.Equals(b);
    }

    public static bool operator ==(Vector3 a, Vector3 b)
    {
        return a.Equals(b);
    }

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z);
    }


    public static Vector3 operator -(Vector3 a)
    {
        return new Vector3(
            -a.X,
            -a.Y,
            -a.Z);
    }

    public static Vector3 operator *(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X * b.X,
            a.Y * b.Y,
            a.Z * b.Z);
    }

    public static Vector3 operator /(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X / b.X,
            a.Y / b.Y,
            a.Z / b.Z);
    }

    public static Vector3 operator %(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
    }

    public static Vector3 operator +(Vector3 a, double b)
    {
        return new Vector3(
            a.X + b,
            a.Y + b,
            a.Z + b);
    }

    public static Vector3 operator -(Vector3 a, double b)
    {
        return new Vector3(
            a.X - b,
            a.Y - b,
            a.Z - b);
    }

    public static Vector3 operator *(Vector3 a, double b)
    {
        return new Vector3(
            a.X * b,
            a.Y * b,
            a.Z * b);
    }

    public static Vector3 operator /(Vector3 a, double b)
    {
        return new Vector3(
            a.X / b,
            a.Y / b,
            a.Z / b);
    }

    public static Vector3 operator %(Vector3 a, double b)
    {
        return new Vector3(a.X % b, a.Y % b, a.Y % b);
    }

    public static Vector3 operator +(double a, Vector3 b)
    {
        return new Vector3(
            a + b.X,
            a + b.Y,
            a + b.Z);
    }

    public static Vector3 operator -(double a, Vector3 b)
    {
        return new Vector3(
            a - b.X,
            a - b.Y,
            a - b.Z);
    }

    public static Vector3 operator *(double a, Vector3 b)
    {
        return new Vector3(
            a * b.X,
            a * b.Y,
            a * b.Z);
    }

    public static Vector3 operator /(double a, Vector3 b)
    {
        return new Vector3(
            a / b.X,
            a / b.Y,
            a / b.Z);
    }

    public static Vector3 operator %(double a, Vector3 b)
    {
        return new Vector3(a % b.X, a % b.Y, a % b.Y);
    }


    public static readonly Vector3 Zero = new(0);

    public static readonly Vector3 One = new(1);


    public static readonly Vector3 Up = new(0, 1, 0);

    public static readonly Vector3 Down = new(0, -1, 0);

    public static readonly Vector3 Left = new(-1, 0, 0);

    public static readonly Vector3 Right = new(1, 0, 0);

    public static readonly Vector3 Backwards = new(0, 0, -1);

    public static readonly Vector3 Forwards = new(0, 0, 1);

    public static readonly Vector3 East = new(1, 0, 0);

    public static readonly Vector3 West = new(-1, 0, 0);

    public static readonly Vector3 North = new(0, 0, -1);

    public static readonly Vector3 South = new(0, 0, 1);

    public bool Equals(Vector3 other)
    {
        return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector3 && Equals((Vector3)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var result = X.GetHashCode();
            result = (result * 397) ^ Y.GetHashCode();
            result = (result * 397) ^ Z.GetHashCode();
            return result;
        }
    }


    #region Math

    public Vector3 Floor()
    {
        return new Vector3(Math.Floor(X), Math.Floor(Y), Math.Floor(Z));
    }

    public Vector3 Round()
    {
        return new Vector3(Math.Round(X), Math.Round(Y), Math.Round(Z));
    }

    public void Clamp(double value)
    {
        if (Math.Abs(X) > value)
            X = value * (X < 0 ? -1 : 1);
        if (Math.Abs(Y) > value)
            Y = value * (Y < 0 ? -1 : 1);
        if (Math.Abs(Z) > value)
            Z = value * (Z < 0 ? -1 : 1);
    }

    public double DistanceTo(Vector3 other)
    {
        return Math.Sqrt(Square(other.X - X) +
                         Square(other.Y - Y) +
                         Square(other.Z - Z));
    }

    private double Square(double num)
    {
        return num * num;
    }

    public const float kEpsilon = 0.00001f;

    public Vector3 Normalize()
    {
        if (Magnitude > kEpsilon)
            return this / Magnitude;
        return Zero;
    }

    public float Magnitude => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public double Distance => DistanceTo(Zero);

    public static Vector3 Min(Vector3 value1, Vector3 value2)
    {
        return new Vector3(
            Math.Min(value1.X, value2.X),
            Math.Min(value1.Y, value2.Y),
            Math.Min(value1.Z, value2.Z)
        );
    }

    public static Vector3 Max(Vector3 value1, Vector3 value2)
    {
        return new Vector3(
            Math.Max(value1.X, value2.X),
            Math.Max(value1.Y, value2.Y),
            Math.Max(value1.Z, value2.Z)
        );
    }

    public static double Dot(Vector3 value1, Vector3 value2)
    {
        return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
    }

    public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
    {
        Cross(ref vector1, ref vector2, out vector1);
        return vector1;
    }

    public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
    {
        var x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
        var y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
        var z = vector1.X * vector2.Y - vector2.X * vector1.Y;
        result.X = x;
        result.Y = y;
        result.Z = z;
    }

    #endregion
}