using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Human : Player
    {

        protected VirtualKeys UpKey;
        protected VirtualKeys DownKey;

        public Human(Game game, PlayerSide side, VirtualKeys UpKey, VirtualKeys DownKey) : base(game, side)
        {
            this.UpKey = UpKey;
            this.DownKey = DownKey;
        }

        public override void Update(double mod)
        {
            this.moveUp = FastConsole.IsKeyDown(this.UpKey);
            this.moveDown = FastConsole.IsKeyDown(this.DownKey);
            base.Update(mod);
        }
    }
}
