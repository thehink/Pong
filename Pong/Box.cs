using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Box : Entity
    {
        public Box(Game game) : base(game)
        {

        }

        public override void Update(double mod)
        {

        }

        public override void Draw(FastConsole cs)
        {
            cs.WriteChar('Ö', this.position.RoundedX, this.position.RoundedY, ConsoleColor.White);
        }
    }
}
