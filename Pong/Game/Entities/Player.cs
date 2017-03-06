using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pong.Game.Entities
{
    enum PlayerType
    {
        Human,
        Bot
    }

    enum PlayerSide
    {
        Left,
        Right
    }

    abstract class Player : Box
    {
        public string Name { get; set; }
        public PlayerType Type { get; }
        public PlayerSide Side { get; set; }
        public double MoveSpeed { get; protected set; }
        public int Score { get; private set; }

        protected double redTime = 0;
        protected bool moveUp = false;
        protected bool moveDown = false;

        public Player(string name, PlayerSide side) : base(1, 6)
        {
            this.Name = name;
            this.Side = side;
            this.Score = 0;
            this.MoveSpeed = 0.6;
        }

        public void Reset()
        {
            this.Score = 0;
            this.ResetPosition();
        }

        public void ResetPosition()
        {
            this.Position.Y = this.game.height / 2 - this.Height/2;
            if(this.Side == PlayerSide.Right)
            {
                this.Position.X = this.game.width - 2;
            }
            if (this.Side == PlayerSide.Left)
            {
                this.Position.X = 1;
            }
        }

        public void OnScore()
        {
            this.Score++;
        }

        public void OnCollide()
        {
            this.redTime = 0;
            this.Color = ConsoleColor.Red;
        }

        public override void Update(double mod)
        {
            base.Update(mod);

            if(this.Color == ConsoleColor.Red && this.redTime > 6)
            {
                this.Color = ConsoleColor.White;
            }else
            {
                this.redTime += mod;
            }

            if (this.moveUp)
            {
                this.Position.Y -= MoveSpeed * mod;
            }

            if (this.moveDown)
            {
                this.Position.Y += MoveSpeed * mod;
            }

            if(this.Position.Y + this.Height > this.game.height - 1)
            {
                this.Position.Y = this.game.height - this.Height - 1;
            }

            if (this.Position.Y < 1)
            {
                this.Position.Y = 1;
            }

        }
    }
}
