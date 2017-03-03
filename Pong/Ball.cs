using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Ball : Entity
    {
        public double Velocity { get; private set; }
        public Vector Direction { get; private set; }


        public Ball(Game game) : base(game)
        {

        }

        public void Reset()
        {
            this.position.X = this.game.width / 2;
            this.position.Y = this.game.height / 2;
        }

        public override void Update(double mod)
        {
            if(this.position.X < this.game.Player1.position.X + 1)
            {
                
            }
        }
   
        public override void Draw(FastConsole cs)
        {
            cs.WriteChar('O', this.position.RoundedX, this.position.RoundedY, ConsoleColor.White);
        }
    }
}
