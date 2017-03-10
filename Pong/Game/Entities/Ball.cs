using Pong.NativeConsole;
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

        protected double ballCountDownTimer = 3;

        public Ball(int width, int height) : base(width, height)
        {
            this.Direction = new Vector2(0, 0);
            this.Velocity = 0;
            this.Color = ConsoleColor.Gray;
        }

        public void ResetPosition()
        {

            ballCountDownTimer = 2.0;

            this.Position.X = this.game.Width / 2;
            this.Position.Y = this.game.Height / 2;

            this.Velocity = 1.9;

            double angle = -Math.PI/2 + 0.2 * Math.PI + 0.6 * this.rand.NextDouble() * Math.PI;

            angle += this.rand.Next(0, 2) == 0 ? Math.PI : 0;

            this.Direction.X = Math.Cos(angle); // this.rand.Next(0, 2) == 1 ? 0.5 : -0.5;
            this.Direction.Y = Math.Sin(angle); //this.rand.Next(0, 2) == 1 ? 0.5 : -0.5;
        }

        public override void Update(double mod)
        {
            base.Update(mod);

            if (this.ballCountDownTimer > 0)
            {
                this.ballCountDownTimer -= mod / 60;
                return;
            }

            //this.Velocity += 0.0001 * mod;

            this.Position.Add(this.Direction.Copy().Multiply(this.Velocity * mod));

            for(int i = 0; i < this.game.Entities.Count; ++i)
            {
                if (this.game.Entities[i].GetType().IsSubclassOf(typeof(Player)))
                {
                    Player pl = (Player)this.game.Entities[i];
                    if (this.IsCollisionWith(pl))
                    {

                        //Fix bug with calculation
                        this.Position.Y -= this.Direction.Y * (pl.Side == PlayerSide.Left ? this.Position.X - 1 : this.Position.X - this.game.Width + 2) / this.Direction.X;

                        this.Direction.X *= -1;
                        //Fix ball getting stuck to a player
                        this.Position.X = pl.Side  == PlayerSide.Left ? pl.Position.X + 1 : pl.Position.X - 2;

                        pl.OnCollide();

                        this.Velocity += 0.05;
                    }
                }
            }


            if (this.Position.X <= 0 || this.Position.X >= this.game.Width - 1)
            {
                this.game.PlayerScored(this.Position.X <= 0 ? this.game.Player2 : this.game.Player1);
            }

            if (this.Position.Y <= 0 || this.Position.Y >= this.game.Height)
            {
                this.Direction.Y *= -1;
            }

            if(this.Position.Y < 0)
            {
                this.Position.X += this.Direction.X * this.Position.Y / this.Direction.Y;
                this.Position.Y = 0;
            }

            if (this.Position.Y > this.game.Height)
            {
                this.Position.X += this.Direction.X * (this.Position.Y - this.game.Height) / this.Direction.Y;
                this.Position.Y = this.game.Height;
            }

            if (this.Position.X < 0)
            {
                this.Position.X = 0;
            }

            if (this.Position.X > this.game.Width)
            {
                this.Position.X = this.game.Width;
            }
        }

        public override void Draw(FastConsole cs)
        {

            if (this.ballCountDownTimer > 0)
            {
                ConsoleColor color;

                int secondsLeft = (int)Math.Round(this.ballCountDownTimer);

                if (secondsLeft >= 3)
                {
                    color = ConsoleColor.Magenta;
                }
                else if (secondsLeft >= 2)
                {
                    color = ConsoleColor.Red;
                }else if (secondsLeft >= 1)
                {
                    color = ConsoleColor.Yellow;
                }else
                {
                    color = ConsoleColor.Green;
                }

                this.game.WriteString(Math.Round(this.ballCountDownTimer).ToString(), this.Position.RoundedX, this.Position.RoundedY, color);
                return;
            }

            base.Draw(cs);
        }

    }
}
