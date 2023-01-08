using Snake;
using SnakeGame.Controller;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Constant = Snake.Constant;

namespace SnakeGame
{
    internal class Apple
    {
        internal static Position CurrentApplePosition { get; private set; }
        private readonly Random _random;
        public Apple()
        {
            _random = new Random();
        }

        internal void CreateApple(char[,] map)
        {
            var x = _random.Next(2, map.GetLength(1) - 1);
            var y = _random.Next(2, map.GetLength(0) - 1);
            if (map[y, x] is not Constant.EmptyCell)
            {
                CreateApple(map);
                return;
            }
            map[y, x] = Constant.AppleDesignation;
            CurrentApplePosition = new Position(x, y);
        }
    }
}
