using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Pong.NativeConsole;
using Pong.Game.Entities;

namespace Pong.Game
{
    class GameInstance
    {
        protected FastConsole cs;
        public Player Player1;
        public Player Player2;
        protected Ball ball;

        public List<Entity> Entities { get; }

        public short width;
        public short height;
        protected bool running;

        public GameInstance(short width, short height)
        {
            Console.CursorVisible = false;

            this.width = width;
            this.height = height;
            this.cs = new FastConsole(width, height);
            this.ball = new Ball(2, 1);
            this.Entities = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            entity.SetGameInstance(this);
            this.Entities.Add(entity);
        }

        public void NewGame(Player player1, Player player2)
        {
            this.Player1 = player1;
            this.Player2 = player2;
            this.Player1.Side = PlayerSide.Left;
            this.Player2.Side = PlayerSide.Right;

            this.Entities.Clear();
            this.AddEntity(this.Player1);
            this.AddEntity(this.Player2);
            this.AddEntity(this.ball);

            NewRound();

            running = true;
            Loop();
        }

        public void PlayerScored(Player player)
        {
            player.OnScore();
            if (player.Score == 5)
            {
                this.EndGame();
            }else
            {
                this.NewRound();
            }
        }

        public void EndGame()
        {
            running = false;
        }

        public void NewRound()
        {
            ball.Reset();
            Player1.Reset();
            Player2.Reset();
        }

        protected void DrawBoard()
        {

            for (int i = 0; i < this.height; ++i)
            {
                this.cs.WriteChar((char)219, 0, i, ConsoleColor.DarkGreen);
                this.cs.WriteChar((char)219, this.width - 1, i, ConsoleColor.DarkGreen);
            }

            for (int i = 0; i < this.width; ++i)
            {
                this.cs.WriteChar((char)219, i, 0, ConsoleColor.Blue);
                this.cs.WriteChar((char)219, i, this.height - 1, ConsoleColor.Blue);
            }

            this.cs.WriteString($"Name: {Player1.Name}", (short)(this.width * 0.15), (short)(this.height * 0.15));
            this.cs.WriteString($"Score: {Player1.Score}", (short)(this.width * 0.15), (short)(this.height * 0.15 + 1));

            this.cs.WriteString($"Name: {Player2.Name}", (short)(this.width * 0.65), (short)(this.height * 0.15));
            this.cs.WriteString($"Score: {Player2.Score}", (short)(this.width * 0.65), (short)(this.height * 0.15 + 1));
        }

        protected void Logic(double mod)
        {
            for(int i = 0; i < this.Entities.Count; ++i)
            {
                this.Entities[i].Update(mod);
                this.Entities[i].Draw(this.cs);
            }
        }

        protected void Loop()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double lastTime = stopwatch.ElapsedMilliseconds;
            double delta = 0;

            while (running)
            {
                delta = stopwatch.ElapsedMilliseconds - lastTime;
                lastTime = stopwatch.ElapsedMilliseconds;
                double deltaMod = delta / (1000.0 * 1.0 / 60.0);
                this.cs.Clear();
                this.DrawBoard();
                this.Logic(deltaMod);
                this.cs.WriteString($"FPS: { (1000.0 / delta) }", 1, 39, ConsoleColor.Cyan);
                this.cs.Draw();
                Thread.Sleep(1);
            }
        }
    }
}
