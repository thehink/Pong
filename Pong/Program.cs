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
            Console.Title = "Pong";
            
            GameInstance game = new GameInstance((int)(Console.LargestWindowWidth/1.5), (int)(Console.LargestWindowHeight/1.5));
            Console.SetWindowPosition(0, 0);
            game.ChangeSettings();
        }
    }
}
