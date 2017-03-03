using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pong
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

    abstract class Player : Entity
    {
        public string Name { get; set; }
        public PlayerType Type { get; }
        public PlayerSide Side { get; set; }
        public double MoveSpeed { get; private set; }
        public int Size { get; private set; }
        public int Score { get; private set; }

        protected bool moveUp = false;
        protected bool moveDown = false;

        public Player(Game game, PlayerSide side) : base(game)
        {
            this.Side = side;
            this.Reset();
        }

        public void Listen()
        {

        }

        public void Reset()
        {
            this.MoveSpeed = 0.3;
            this.Score = 0;
            this.Size = 6;
            this.position.Y = this.game.height / 2 - this.Size/2;
            if(this.Side == PlayerSide.Right)
            {
                this.position.X = 69;
            }
            if (this.Side == PlayerSide.Left)
            {
                this.position.X = 0;
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

            if(this.position.Y + this.Size > this.game.height - 1)
            {
                this.position.Y = this.game.height - this.Size - 1;
            }

            if (this.position.Y < 1)
            {
                this.position.Y = 1;
            }

        }

        public override void Draw(FastConsole cs)
        {
            //cs.WriteString($"{this.position.RoundedY}                 ", 1, 35, ConsoleColor.White);
            for(int i = 0; i < this.Size; ++i)
            {
                cs.WriteChar((char)219, this.position.RoundedX, this.position.RoundedY + i, ConsoleColor.White);
            }
            
        }
    }
}
