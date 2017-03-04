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
        public double MoveSpeed { get; private set; }
        public int Score { get; private set; }

        protected bool moveUp = false;
        protected bool moveDown = false;

        public Player(string name) : base(1, 6)
        {
            this.Name = name;
            this.Score = 0;
        }

        public void Reset()
        {
            this.MoveSpeed = 0.3;
            this.position.Y = this.game.height / 2 - this.Height/2;
            if(this.Side == PlayerSide.Right)
            {
                this.position.X = this.game.width - 2;
            }
            if (this.Side == PlayerSide.Left)
            {
                this.position.X = 1;
            }
        }

        public void OnScore()
        {
            this.Score++;
        }

        public override void Update(double mod)
        {
            if (this.moveUp)
            {
                this.position.Y -= MoveSpeed * mod;
            }

            if (this.moveDown)
            {
                this.position.Y += MoveSpeed * mod;
            }

            if(this.position.Y + this.Height > this.game.height - 1)
            {
                this.position.Y = this.game.height - this.Height - 1;
            }

            if (this.position.Y < 1)
            {
                this.position.Y = 1;
            }

        }
    }
}
