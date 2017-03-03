using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(100, 40);

            Human player1 = new Human("Player1", VirtualKeys.A, VirtualKeys.Z);
            Human player2 = new Human("Player2", VirtualKeys.Up, VirtualKeys.Down);

            game.NewGame(player1, player2);

            Console.ReadKey();
        }
    }
}
