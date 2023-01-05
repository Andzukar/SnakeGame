using Snake;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Constant = Snake.Constant;

namespace SnakeGame
{
    internal class Apple
    {
        private readonly Random _random;
        public Apple()
        {
            _random = new Random();
        }

        internal void CreateApple(char[,] map)
        {
            var x = _random.Next(1, map.GetLength(0) - 1);
            var y = _random.Next(1, map.GetLength(1) - 1);
            if (map[x,y] is Constant.SnakeDesignation 
             || map[x, y] is Constant.MapBorderDesignation)
            {
                CreateApple(map);
                return;
            }
            map[x, y] = Constant.AppleDesignation;
        }
    }
}
