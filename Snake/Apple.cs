using Snake;
using System.ComponentModel;

namespace SnakeGame
{
    internal class Apple
    {
        private Random _random;
        public Apple()
        {
            _random = new Random();
        }

        internal void CreateApple(char[,] map)
        {
            var x = _random.Next(1, map.GetLength(0) - 1);
            var y = _random.Next(1, map.GetLength(1) - 1);
            map[x, y] = Constant.AppleDesignation;
        }
    }
}
