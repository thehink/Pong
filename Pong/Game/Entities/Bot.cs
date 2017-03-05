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

        public Bot(string name, PlayerSide side, int thinkAheadSteps = 8) : base(name, side)
        {
            this.ThinkAheadSteps = thinkAheadSteps;
        }

        public Vector2 CalcNextBouncePoint(Vector2 pos, Vector2 dir)
        {
            double deltaY = dir.Y > 0 ? this.game.height - 2 - pos.Y : -pos.Y + 1;
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
                    x > wallX && nextBouncePoint.X >= x && BallDir.X > 0 ||
                    x < wallX && nextBouncePoint.X <= x && BallDir.X < 0)
                {
                    return FindIntersection(BallPos, BallDir, x);
                }else if (
                    x < wallX && nextBouncePoint.X >= wallX && BallDir.X > 0 ||
                    x > wallX && nextBouncePoint.X <= wallX && BallDir.X < 0)
                {
                    nextBouncePoint.Set(FindIntersection(BallPos, BallDir, wallX));
                    BallDir.X *= -1;
                }
                else
                {
                    BallDir.Y *= -1;
                }

                BallPos.Set(nextBouncePoint);
            }

            return new Vector2(double.MinValue, double.MinValue);
        }

        public override void Update(double mod)
        {
            Vector2 next = this.GetNextIntersectionAtX(this.Side == PlayerSide.Left ? 2 : this.game.width - 3, this.Side == PlayerSide.Right ? 2 : this.game.width - 3);

            if(next.Y > 0)
            {
                this.moveDown = this.Position.Y + this.Height / 2 - 1 < next.Y;
                this.moveUp = this.Position.Y + this.Height / 2 - 1 > next.Y;
            }
            else
            {
                this.moveDown = this.Position.Y + this.Height / 2 < this.game.height / 2;
                this.moveUp = this.Position.Y + this.Height / 2 > this.game.height / 2;
            }

            base.Update(mod);
        }


        public override void Draw(FastConsole cs)
        {
            base.Draw(cs);

            Vector2 next = this.GetNextIntersectionAtX(this.game.width - 3, 2);
            if(next.RoundedX > 0 && next.RoundedX < this.game.width &&
                next.RoundedY > 0 && next.RoundedY < this.game.height)
            {
                //cs.WriteChar('H', next.RoundedX, next.RoundedY, ConsoleColor.Red);
            }
            
        }
    }
}
