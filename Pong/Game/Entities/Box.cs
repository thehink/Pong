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

        public Box(int width, int height) : base()
        {
            this.Width = width;
            this.Height = height;
            this.Character = (char)219;
            this.Color = ConsoleColor.White;
        }

        public override void Update(double mod)
        {

        }

        public bool IsCollisionWith(Box box)
        {
            return this.Position.X + this.Width > box.Position.X &&
               this.Position.Y + this.Height > box.Position.Y &&
               this.Position.X < box.Position.X + box.Width &&
               this.Position.Y < box.Position.Y + box.Height;
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
