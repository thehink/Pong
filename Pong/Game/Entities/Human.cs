using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pong.NativeConsole;

namespace Pong.Game.Entities
{
    class Human : Player
    {

        protected VirtualKeys UpKey;
        protected VirtualKeys DownKey;

        public Human(string name, VirtualKeys UpKey, VirtualKeys DownKey) : base(name)
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
