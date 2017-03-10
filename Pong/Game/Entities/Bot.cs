using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pong.NativeConsole;

namespace Pong.Game.Entities
{
    class Bot : Player
    {

        private int ThinkAheadSteps; //number of bounces the bot will think ahead

        public Bot(string name, PlayerSide side, int difficulty = 3) : base(name, side)
        {

            if(difficulty == 1)
            {
                this.MoveSpeed = 0.2;
                this.ThinkAheadSteps = 2;
            }
            else if (difficulty == 2)
            {
                this.MoveSpeed = 0.2;
                this.ThinkAheadSteps = 3;
            }
            else if (difficulty == 3)
            {
                this.MoveSpeed = 0.6;
                this.ThinkAheadSteps = 10;
            }
            
        }

        public Vector2 CalcNextBouncePoint(Vector2 pos, Vector2 dir)
        {
            double deltaY = dir.Y > 0 ? this.game.Height - pos.Y : -pos.Y;
            double deltaX = dir.X * deltaY / dir.Y;
            return new Vector2(pos.X + deltaX, pos.Y + deltaY);
        }

        public Vector2 FindIntersection(Vector2 pos, Vector2 dir, double x)
        {
            double deltaX =  x - pos.X;
            double deltaY = dir.Y * deltaX / dir.X;
            return new Vector2(pos.X + deltaX, pos.Y + deltaY);
        }

        public Vector2 GetNextIntersectionAtX(double x, double wallX)
        {

            Vector2 BallPos = this.game.ball.Position.Copy();
            Vector2 BallDir = this.game.ball.Direction.Copy();

            for (int i = 0; i < ThinkAheadSteps; ++i)
            {

                Vector2 nextBouncePoint = CalcNextBouncePoint(BallPos, BallDir);

                if (
                    nextBouncePoint.X == double.NegativeInfinity ||
                    nextBouncePoint.X == double.PositiveInfinity ||
                    x >= wallX && nextBouncePoint.X >= x && BallDir.X > 0 ||
                    x <= wallX && nextBouncePoint.X <= x && BallDir.X < 0)
                {

                    Vector2 inter = FindIntersection(BallPos, BallDir, x);

                    this.game.WriteDebug($"F {Math.Round(inter.X, 2)} {Math.Round(inter.Y, 2)}", inter.RoundedX, inter.RoundedY, ConsoleColor.Red);
                    return inter;
                }else if (
                    x <= wallX && nextBouncePoint.X >= wallX && BallDir.X > 0 ||
                    x >= wallX && nextBouncePoint.X <= wallX && BallDir.X < 0)
                {
                    nextBouncePoint.Set(FindIntersection(BallPos, BallDir, wallX));
                    BallDir.X *= -1;
                }
                else
                {
                    BallDir.Y *= -1;
                }

                this.game.WriteDebug($"H {Math.Round(nextBouncePoint.X, 2)} {Math.Round(nextBouncePoint.Y, 2)}", nextBouncePoint.RoundedX, nextBouncePoint.RoundedY, ConsoleColor.Red);

                BallPos.Set(nextBouncePoint);
            }

            return new Vector2(double.MinValue, double.MinValue);
        }

        public override void Update(double mod)
        {
            Vector2 next = this.GetNextIntersectionAtX(this.Side == PlayerSide.Left ? 1 : this.game.Width - 2, this.Side == PlayerSide.Right ? 1 : this.game.Width - 2);

            if(next.Y > -1)
            {
                this.moveDown = this.Position.Y + this.Height / 2 < Math.Round(next.Y);
                this.moveUp = this.Position.Y + this.Height / 2 > Math.Round(next.Y);
            }
            else
            {
                this.moveDown = this.Position.Y + this.Height / 2 < this.game.Height / 2;
                this.moveUp = this.Position.Y + this.Height / 2 > this.game.Height / 2;
            }

            base.Update(mod);
        }


        public override void Draw(FastConsole cs)
        {
            base.Draw(cs);

            //Vector2 next = this.GetNextIntersectionAtX(this.game.Width, 0);
            //this.game.WriteChar('H', next.RoundedX, next.RoundedY, ConsoleColor.Red);

        }
    }
}
