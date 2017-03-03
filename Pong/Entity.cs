using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    abstract class Entity
    {
        public Vector position;
        protected Game game;

        public Entity(Game game)
        {
            this.game = game;
            this.position = new Vector(0, 0);
        }

        public abstract void Update(double mod);
        public abstract void Draw(FastConsole cs);
    }
}
