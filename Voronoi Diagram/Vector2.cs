using System;
using System.Windows;

namespace Voronoi_Diagram
{
    /// <summary>
    /// Vector class by Tobias Bollinger
    /// </summary>
    public class Vector2
    {

        //Copy contructor
        //Scale	Multiplies two vectors component-wise.
        //operator !=	Returns true if vectors different.
        //operator ==	Returns true if the vectors are equal.
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Vector2(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static explicit operator Vector2(Point p)
        {
            return new Vector2(p.X, p.Y);
        }
        
        public double Magnitude => Math.Sqrt(X * X + Y * Y);
        public double MagnitudeSqr => X * X + Y * Y;
        public double ManhattanDistance => Math.Abs(X) + Math.Abs(Y);
        public Vector2 Normalized => new Vector2(X / Magnitude, Y / Magnitude);

        public static Vector2 Up = new Vector2(1, 0);
        public static Vector2 Down = new Vector2(-1, 0);
        public static Vector2 Left = new Vector2(0, 1);
        public static Vector2 Right = new Vector2(0, -1);
        public static Vector2 Zero = new Vector2(0, 0);

        public override string ToString()
        {
            return $"[{X};{Y}]";
        }

        #region opperations

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static Vector2 operator *(Vector2 v, double d)
        {
            return new Vector2(v.X * d, v.Y * d);
        }

        public static Vector2 operator *(double d, Vector2 v)
        {
            return new Vector2(v.X * d, v.Y * d);
        }

        public static Vector2 operator /(Vector2 v, double d)
        {
            return v * (1 / d);
        }

        /// <summary>
        /// Calculates the dot product
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Dot Product</returns>
        public static double Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static double operator *(Vector2 v1, Vector2 v2)
        {
            return Dot(v1, v2);
        }

        /// <summary>
        /// Calculates the angle between 2 Vectors in radian
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Angle in radian</returns>
        public static double Angle(Vector2 v1, Vector2 v2)
        {
            return Math.Atan2(v1.X, v1.Y) - Math.Atan2(v2.X, v2.Y);
        }

        /// <summary>
        /// Calculates the cross procuct between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Cross product</returns>
        public static double Cross(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v2.X * v1.Y;
        }

        /// <summary>
        /// Linera interpolates between Vector a and Vector b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <param name="t">Time</param>
        /// <returns>When t is 0 returns a, when t is 1 returns b</returns>
        public static Vector2 Lerp(Vector2 a, Vector2 b, double t)
        {
            return new Vector2
            {
                X = a.X * (1 - t) + b.X * t,
                Y = a.Y * (1 - t) + b.Y * t
            };
        }

        /// <summary>
        /// Calculates the distance between 2 Vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Distance(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).Magnitude;
        }

        /// <summary>
        /// Calculates the squared distance between 2 Vecotors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double DistanceSqrt(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).MagnitudeSqr;
        }

        public static double ScalarProjection(Vector2 v1, Vector2 v2)
        {
            //| b | cos(theta)
            var angle = Vector2.Angle(v1, v2);
            return v1.Magnitude * Math.Cos(angle);
        }

        public static Vector2 Projection(Vector2 v1, Vector2 v2)
        {
            return ((v1 * v2) / v2.MagnitudeSqr) * v2;
        }

        /// <summary>
        /// 1 : falls P links von QR liegt oder auf QR nach R folgt
        /// 0 : falls P zwischen Q und R liegt
        /// -1: falls P rechts von QR liegt oder auf QR vor Q kommt
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns>
        /// 1 : falls P links von QR liegt oder auf QR nach R folgt
        /// 0 : falls P zwischen Q und R liegt
        /// -1: falls P rechts von QR liegt oder auf QR vor Q kommt
        /// </returns>
        //public static int CounterClockWise(Vector2 v, Vector2 q, Vector2 r)
        //{

        //}

        /// <summary>
        /// Reflects a vector off the vector defined by a normal.
        /// </summary>
        /// <param name="inVector">The Vector to reflect</param>
        /// <param name="inNormal">The normal as refction normal</param>
        /// <returns>The reflected vector</returns>
        public static Vector2 Reflect(Vector2 inVector, Vector2 inNormal)
        {
            return inVector - 2 * (inVector * inNormal) * inNormal;
        }

        public bool IsBetween(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).ManhattanDistance >= v1.ManhattanDistance + v2.ManhattanDistance;
        }

        public double Area2(Vector2 v1, Vector2 v2)
        {
            return Vector2.Cross(v1 - this, v2 - this);
        }
        public bool IsConvex(Vector2 p0, Vector2 p1)
        {
            var f = Area2(p0, p1);
            return f < 0 || f == 0 && !IsBetween(p0, p1);
        }

        public bool IsLess(Vector2 v)
        {
            var f = Vector2.Cross(this, v);
            return f > 0 || f == 0 && IsFurther(v);
        }

        public bool IsFurther(Vector2 p) => ManhattanDistance > p.ManhattanDistance;

        #endregion
    }

    public static class MathHelper
    {
        public static double RadianToDegree(double radian)
        {
            return radian * (180 / Math.PI);
        }
    }
}
