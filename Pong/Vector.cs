using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Vector
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


        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector Normalize()
        {
            return new Vector(0,0);
        }
    }
}
