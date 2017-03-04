using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pong.NativeConsole;
using Pong.Game;
using Pong.Game.Entities;

namespace Pong
{
    class Program
    {
        static void Main(string[] args)
        {
            GameInstance game = new GameInstance(100, 40);
            game.ChangeSettings();
        }
    }
}
