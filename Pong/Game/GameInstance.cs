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
    class GameInstance : IDisposable
    {
        public FastConsole cs;

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public Ball ball { get; }

        public List<Entity> Entities { get; }

        public short width;
        public short height;
        protected bool running;

        public GameInstance(short width, short height)
        {
            this.width = width;
            this.height = height;
            this.cs = new FastConsole(width, height);
            
            this.ball = new Ball(2, 1);
            this.Entities = new List<Entity>();
        }

        public void Dispose()
        {
            this.cs.Dispose();
        }

        public void AddEntity(Entity entity)
        {
            entity.SetGameInstance(this);
            this.Entities.Add(entity);
        }

        public void ChangeSettings()
        {
            this.cs.Clear();
            this.cs.Draw();

            Console.SetCursorPosition(0, 0);

            Console.CursorVisible = true;

            Console.Write($"Player 1 Name: ");
            string Player1Name = Console.ReadLine();
            Console.Write($"Is Player 1 Bot(Y/N)");
            bool IsPlayer1Bot = Console.ReadLine().ToLower() == "y";

            Console.Write($"Player 2 Name: ");
            string Player2Name = Console.ReadLine();
            Console.Write($"Is Player 2 Bot(Y/N)");
            bool IsPlayer2Bot = Console.ReadLine() == "y";


            Console.CursorVisible = false;

            if (IsPlayer1Bot)
            {
                this.Player1 = new Bot($"{Player1Name} (BOT)", PlayerSide.Left);
            }else
            {
                this.Player1 = new Human(Player1Name, PlayerSide.Left, VirtualKeys.A, VirtualKeys.Z);
            }

            if (IsPlayer2Bot)
            {
                this.Player2 = new Bot($"{Player2Name} (BOT)", PlayerSide.Right);
            }
            else
            {
                this.Player2 = new Human(Player2Name, PlayerSide.Right, VirtualKeys.Up, VirtualKeys.Down);
            }

            this.Entities.Clear();
            this.AddEntity(this.Player1);
            this.AddEntity(this.Player2);
            this.AddEntity(this.ball);

            this.NewGame();
        }

        public void NewGame()
        {

            this.Player1.Reset();
            this.Player2.Reset();

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

        public void NewRound()
        {
            ball.ResetPosition();
            Player1.ResetPosition();
            Player2.ResetPosition();
        }

        public void EndGame()
        {
            running = false;

            this.DrawEndGameScreen();

            bool invalidKey = true;
            while (invalidKey)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.R:
                        this.NewGame();
                        invalidKey = false;
                        break;
                    case ConsoleKey.N:
                        this.ChangeSettings();
                        invalidKey = false;
                        break;
                    case ConsoleKey.Q:
                        invalidKey = false;
                        break;
                }
            }
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

        protected void DrawEndGameScreen()
        {
            Player winner = Player1.Score > Player2.Score ? Player1 : Player2;
            Player looser = Player1.Score > Player2.Score ? Player2 : Player1;

            this.cs.Clear();
            this.cs.WriteString($"Congratulations {winner.Name}", (short)(this.width * 0.37), (short)(this.height * 0.2));
            this.cs.WriteString($"{winner.Name}: {winner.Score} points", (short)(this.width * 0.4), (short)(this.height * 0.2 + 2), ConsoleColor.Yellow);
            this.cs.WriteString($"{looser.Name}: {looser.Score} points", (short)(this.width * 0.4), (short)(this.height * 0.2 + 3), ConsoleColor.Gray);

            this.cs.WriteString($"Press [R] to play again!", (short)(this.width * 0.37), (short)(this.height * 0.2 + 8), ConsoleColor.DarkGreen);
            this.cs.WriteString($"Press [N] to change settings!", (short)(this.width * 0.37), (short)(this.height * 0.2 + 9), ConsoleColor.DarkGray);
            this.cs.WriteString($"Press [Q] to quit!", (short)(this.width * 0.37), (short)(this.height * 0.2 + 10), ConsoleColor.DarkRed);
            this.cs.Draw();
        }

        protected void Update(double mod)
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
                this.Update(deltaMod);
                this.cs.WriteString($"FPS: { (1000.0 / delta) }", 1, 39, ConsoleColor.Cyan);
                this.cs.Draw();
                Thread.Sleep(1);
                if (FastConsole.IsKeyDown(VirtualKeys.R))
                {
                    this.NewRound();
                }
                if(FastConsole.IsKeyDown(VirtualKeys.E))
                {
                    this.EndGame();
                }
            }
        }
    }
}
