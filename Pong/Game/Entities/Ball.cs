using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Game.Entities
{
    class Ball : Box
    {
        public double Velocity { get; private set; }
        public Vector2 Direction { get; private set; }

        protected Random rand = new Random();

        public Ball(int width, int height) : base(width, height)
        {
            this.Direction = new Vector2(0, 0);
            this.Velocity = 0;
        }

        public void ResetPosition()
        {
            this.Position.X = this.game.width / 2;
            this.Position.Y = this.game.height / 2;

            this.Velocity = 0.9;

            double angle = this.rand.NextDouble() * Math.PI * 2;

            this.Direction.X = this.rand.Next(0, 2) == 1 ? 0.5 : -0.5;
            this.Direction.Y = this.rand.Next(0, 2) == 1 ? 0.5 : -0.5;
        }

        public override void Update(double mod)
        {

            //this.Velocity += 0.0001 * mod;

            this.Position.Add(this.Direction.Copy().Multiply(this.Velocity * mod));

            for(int i = 0; i < this.game.Entities.Count; ++i)
            {
                if (this.game.Entities[i].GetType().IsSubclassOf(typeof(Player)))
                {
                    Player pl = (Player)this.game.Entities[i];
                    if (this.IsCollisionWith(pl))
                    {
                        this.Direction.X *= -1;

                        //Fix ball getting stuck to a player
                        this.Position.X = pl.Side  == PlayerSide.Left ? pl.Position.X + 1 : pl.Position.X - 2;

                        pl.OnCollide();

                        this.Velocity += 0.05;
                    }
                }
            }


            if (this.Position.X <= 1 || this.Position.X >= this.game.width - 3)
            {
                //this.Direction.X *= -1;
                this.game.PlayerScored(this.Position.X <= 1 ? this.game.Player2 : this.game.Player1);
            }

            if (this.Position.Y <= 1 || this.Position.Y >= this.game.height - 2)
            {
                this.Direction.Y *= -1;
            }

            if(this.Position.Y < 1)
            {
                this.Position.Y = 1;
            }

            if (this.Position.Y > this.game.height - 2)
            {
                this.Position.Y = this.game.height - 2;
            }

            if (this.Position.X < 1)
            {
                //this.Position.X = 1;
            }

            if (this.Position.X > this.game.width - 3)
            {
                //this.Position.X = this.game.width - 3;
            }
        }
  
    }
}
