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
            Game game = new Game(70, 40);


            Human player1 = new Human(game, PlayerSide.Left, VirtualKeys.Up, VirtualKeys.Down);
            Human player2 = new Human(game, PlayerSide.Right, VirtualKeys.A, VirtualKeys.Z);

            game.NewGame(player1, player2);

            Console.ReadKey();
        }
    }
}
