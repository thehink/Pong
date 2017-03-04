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


        public Bot(string name, PlayerSide side) : base(name, side)
        {

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

        public Vector2 GetNextIntersectionAtX(double x)
        {

            Vector2 BallPos = this.game.ball.Position.Copy();
            Vector2 BallDir = this.game.ball.Direction.Copy();

            //bounce imaginary ball 4 times and see if we can find an intersection
            for (int i = 0; i < 4; ++i)
            {

                Vector2 nextBouncePoint = CalcNextBouncePoint(BallPos, BallDir);

                if (nextBouncePoint.X >= x && BallDir.X > 0 || nextBouncePoint.X <= x && BallDir.X < 0)
                {
                    return FindIntersection(BallPos, BallDir, x);
                }

                BallDir.Y *= -1;
                BallPos.Set(nextBouncePoint);
            }

            return null;
        }

        public override void Update(double mod)
        {
            Vector2 next = this.GetNextIntersectionAtX(this.Side == PlayerSide.Left ? 2 : this.game.width - 3);

            if(this.Side == PlayerSide.Left && this.game.ball.Direction.X < 0 ||
               this.Side == PlayerSide.Right && this.game.ball.Direction.X > 0)
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

        /*
        public override void Draw(FastConsole cs)
        {
            base.Draw(cs);

            Vector2 next = this.GetNextIntersectionAtX(this.game.width - 3);
            if(next != null && next.RoundedX > 0 && next.RoundedX < this.game.width &&
                next.RoundedY > 0 && next.RoundedY < this.game.height)
            {
                cs.WriteChar('H', next.RoundedX, next.RoundedY, ConsoleColor.Red);
            }
            
        }*/
    }
}
