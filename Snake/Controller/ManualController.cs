using SnakeGame.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Controller
{
    internal class ManualController : IMove
    {
        private ConsoleKey? _key = null;
        private void ReadKeyFromKeybourd()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    _key = Console.ReadKey().Key;
                }
            });
        }

        public void MakeStep(ref int x, ref int y)
        {
            ReadKeyFromKeybourd();
            if (_key.HasValue)
            {
                switch (_key.Value)
                {
                    case ConsoleKey.LeftArrow:
                        x--;
                        break;
                    case ConsoleKey.RightArrow:
                        x++;
                        break;
                    case ConsoleKey.UpArrow:
                        y--;
                        break;
                    case ConsoleKey.DownArrow:
                        y++;
                        break;
                }
            }
        }
    }
}
