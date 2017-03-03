﻿using System;
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

        public Vector2 Normalize()
        {
            return new Vector2(0,0);
        }
    }
}
