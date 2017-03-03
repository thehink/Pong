﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    abstract class Entity
    {
        public Vector2 position;
        protected Game game;

        public Entity()
        {
            this.position = new Vector2(0, 0);
        }

        public void SetGameInstance(Game game)
        {
            this.game = game;
        }

        public abstract void Update(double mod);
        public abstract void Draw(FastConsole cs);
    }
}
