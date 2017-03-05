using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int RoundedX { get
            {
                return (int)Math.Round(this.X);
            }
        }
        public int RoundedY
        {
            get
            {
                return (int)Math.Round(this.Y);
            }
        }


        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2 operator +(Vector2 c1, Vector2 c2)
        {
            return new Vector2(c1.X + c2.X, c1.Y + c2.Y);
        }

        public static Vector2 operator -(Vector2 c1, Vector2 c2)
        {
            return new Vector2(c1.X - c2.X, c1.Y - c2.Y);
        }

        public static Vector2 operator /(Vector2 c1, double val)
        {
            return new Vector2(c1.X / val, c1.Y / val);
        }

        public static Vector2 operator *(Vector2 c1, double val)
        {
            return new Vector2(c1.X * val, c1.Y * val);
        }


        public Vector2 Copy()
        {
            return new Vector2(this.X, this.Y);
        }

        public Vector2 Add(Vector2 vec)
        {
            this.X += vec.X;
            this.Y += vec.Y;
            return this;
        }

        public Vector2 Multiply(double n)
        {
            this.X *= n;
            this.Y *= n;
            return this;
        }

        public Vector2 Set(Vector2 vec)
        {
            this.X = vec.X;
            this.Y = vec.Y;
            return this;
        }
    }
}
