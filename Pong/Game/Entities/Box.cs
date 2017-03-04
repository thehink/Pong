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

        public Box(int width, int height) : base()
        {
            this.Width = width;
            this.Height = height;
            this.Character = (char)219;
        }

        public override void Update(double mod)
        {

        }

        public bool IsCollisionWith(Box box)
        {
            return this.position.X + this.Width > box.position.X &&
               this.position.Y + this.Height > box.position.Y &&
               this.position.X < box.position.X + box.Width &&
               this.position.Y < box.position.Y + box.Height;
        }

        public override void Draw(FastConsole cs)
        {
            for(int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    cs.WriteChar(Character, this.position.RoundedX + x, this.position.RoundedY + y, ConsoleColor.White);
                }
            }
        }
    }
}
