using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pong.NativeConsole;

namespace Pong.Game.Entities
{
    class Box : Entity
    {

        public int Width { get; set; }
        public int Height { get; set; }
        public char Character { get; set; }
        public ConsoleColor Color { get; set; }

        protected Vector2 lastPosition;

        public Box(int width, int height) : base()
        {
            this.Width = width;
            this.Height = height;
            this.Character = (char)219;
            this.Color = ConsoleColor.White;
        }

        public override void Update(double mod)
        {
            this.lastPosition = this.Position.Copy();
        }

        public bool IsCollisionWith(Box box)
        {
            /*return this.Position.X + this.Width > box.Position.X &&
                   this.Position.Y + this.Height > box.Position.Y &&
                   this.Position.X < box.Position.X + box.Width &&
                   this.Position.Y < box.Position.Y + box.Height;*/

            double DeltaX = this.Position.X - this.lastPosition.X;
            double DeltaY = this.Position.Y - this.lastPosition.Y;

            double AbsDeltaX = Math.Abs(DeltaX);
            double AbsDeltaY = Math.Abs(DeltaY);

            //double moveDiv = DeltaY != 0 ? Math.Abs(DeltaX / DeltaY) : 0;
            double max = Math.Max(AbsDeltaX, AbsDeltaY);
            double min = Math.Min(AbsDeltaX, AbsDeltaY);

            double div = min / max;



            while (min > 0.001 || max > 0.001)
            {
                //double moveX = (Math.Abs(DeltaX) > 1 ? (DeltaX > 0 ? 1 : -1) : DeltaX);
                //double moveY = moveDiv * (Math.Abs(DeltaY) > 1 ? (DeltaY > 0 ? 1 : -1) : DeltaY);

                double moveMax = max > 1 ? 1 : max;
                double moveMin = div * (min > 1 ? 1 : min);

                double xxx = AbsDeltaX > AbsDeltaY ? (DeltaX > 0 ? moveMax : -moveMax) : (DeltaX > 0 ? moveMin : -moveMin);
                double yyy = AbsDeltaX > AbsDeltaY ? (DeltaY > 0 ? moveMin : -moveMin) : (DeltaY > 0 ? moveMax : -moveMax);

                if (this.Position.X - DeltaX + this.Width > box.Position.X &&
                   this.Position.Y - DeltaY + this.Height > box.Position.Y &&
                   this.Position.X - DeltaX < box.Position.X + box.Width &&
                   this.Position.Y - DeltaY < box.Position.Y + box.Height)
                {
                    return true;
                }

                DeltaX -= xxx;
                DeltaY -= yyy;

                max -= moveMax;
                min -= moveMin;
            }

            return false;
        }

        public override void Draw(FastConsole cs)
        {
            for(int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    cs.WriteChar(Character, this.Position.RoundedX + x, this.Position.RoundedY + y, this.Color);
                }
            }

        }
    }
}
