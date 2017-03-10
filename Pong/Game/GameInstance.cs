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

        protected int ScoreLimit { get; private set; }

        public FastConsole cs { get; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public Ball ball { get; }

        public List<Entity> Entities { get; }
        public int Width { get; }
        public int Height { get; }
        public int ScreenWidth { get; }
        public int ScreenHeight { get; }

        public bool IsDebug { get; } = false;

        protected short width;
        protected short height;
        protected bool running;

        public GameInstance(int width, int height)
        {
            this.ScreenWidth = width;
            this.ScreenHeight = height;

            this.Width = width - 3;
            this.Height = height - 3;

            this.width = (short)width;
            this.height = (short)height;
            this.cs = new FastConsole((short)(this.ScreenWidth), (short)(this.ScreenHeight));

            this.ScoreLimit = 5;
            
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

            string Player1Name;
            string Player2Name;
            bool IsPlayer1Bot;
            bool IsPlayer2Bot;
            int Bot1Difficulty = 0;
            int Bot2Difficulty = 0;

            this.cs.Clear();
            this.cs.Draw();

            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;

            Console.Write($"Score Limit ");
            this.ScoreLimit = FastConsole.ReadInt(1, 50);

            Console.Write($"Player 1 Name: ");
            Player1Name = Console.ReadLine();
            Console.Write($"Is Player 1 Bot(Y/N)");
            IsPlayer1Bot = Console.ReadLine().ToLower() == "y";
            

            if (IsPlayer1Bot)
            {
                Console.Write($"Difficulty ");
                Bot1Difficulty = FastConsole.ReadInt(1, 3);
            }

            Console.Write($"Player 2 Name: ");
            Player2Name = Console.ReadLine();
            Console.Write($"Is Player 2 Bot(Y/N)");
            IsPlayer2Bot = Console.ReadLine() == "y";

            if (IsPlayer2Bot)
            {
                Console.Write($"Difficulty ");
                Bot2Difficulty = FastConsole.ReadInt(1, 3);
            }


            Console.CursorVisible = false;

            if (IsPlayer1Bot)
            {
                this.Player1 = new Bot($"{Player1Name} (BOT)", PlayerSide.Left, Bot1Difficulty);
            }else
            {
                this.Player1 = new Human(Player1Name, PlayerSide.Left, VirtualKeys.A, VirtualKeys.Z);
            }

            if (IsPlayer2Bot)
            {
                this.Player2 = new Bot($"{Player2Name} (BOT)", PlayerSide.Right, Bot2Difficulty);
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
            Thread.Sleep(1000);
            if (player.Score == this.ScoreLimit)
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

            while (Console.KeyAvailable)
                Console.ReadKey(true);

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

            for (int i = 0; i < this.ScreenHeight; ++i)
            {
                this.cs.WriteChar((char)219, 0, i, ConsoleColor.DarkGreen);
                this.cs.WriteChar((char)219, this.ScreenWidth - 1, i, ConsoleColor.DarkGreen);
            }

            for (int i = 0; i < this.ScreenWidth; ++i)
            {
                this.cs.WriteChar((char)219, i, 0, ConsoleColor.Blue);
                this.cs.WriteChar((char)219, i, this.ScreenHeight - 1, ConsoleColor.Blue);
            }

            this.cs.WriteString($"Name: {Player1.Name}", (short)(this.width * 0.15), (short)(this.height * 0.15));
            this.cs.WriteString($"Score: {Player1.Score}/{this.ScoreLimit}", (short)(this.width * 0.15), (short)(this.height * 0.15 + 1));

            this.cs.WriteString($"Name: {Player2.Name}", (short)(this.width * 0.65), (short)(this.height * 0.15));
            this.cs.WriteString($"Score: {Player2.Score}/{this.ScoreLimit}", (short)(this.width * 0.65), (short)(this.height * 0.15 + 1));
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

        public void WriteChar(char chr, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            if(x >= 0 && x <= this.Width && y >= 0 && y <= this.Height)
            {
                this.cs.WriteChar(chr, x + 1, y + 1, color);
            }
        }

        public void WriteString(string str, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            if (x >= 0 && x <= this.Width && y >= 0 && y <= this.Height)
            {
                this.cs.WriteString(str, x + 1, y + 1, color);
            }
        }

        public void WriteDebug(string str, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            if (this.IsDebug)
            {
                this.WriteString(str, x, y, color);
            }
        }

        protected void Update(double mod)
        {
            for (int i = 0; i < this.Entities.Count; ++i)
            {
                this.Entities[i].Update(mod);
                this.Entities[i].Draw(this.cs);
            }
        }

        protected void Loop()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            long lastUpdate = stopwatch.ElapsedMilliseconds;
            double delta = 0;

            int MAX_UPDATES_PER_SECOND = 120;
            int MIN_WAIT_TICKS = 1000 / MAX_UPDATES_PER_SECOND;

            while (running)
            {
                while (stopwatch.ElapsedMilliseconds < lastUpdate + MIN_WAIT_TICKS)
                {
                    Thread.Sleep(0);
                }

                delta = stopwatch.ElapsedMilliseconds - lastUpdate;
                lastUpdate = stopwatch.ElapsedMilliseconds;
                double deltaMod = delta / (1000.0 * 1.0 / 60.0);
                this.cs.Clear();
                this.DrawBoard();
                this.Update(deltaMod);
                this.cs.WriteString($"FPS: { Math.Round(1000.0 / delta, 2) }", 1, this.height - 1, ConsoleColor.Cyan);
                //this.WriteChar('A', this.Width, this.Height);
                this.cs.Draw();

                if (FastConsole.IsKeyDown(VirtualKeys.R))
                {
                    this.NewRound();
                }
                if (FastConsole.IsKeyDown(VirtualKeys.E))
                {
                    this.EndGame();
                }
            }
        }
    }
}
