using Snake;
using System.Runtime.CompilerServices;

namespace SnakeGame
{
    internal class Snake
    {
        private Dictionary<int, SnakeItem> _snakeParts;
        private readonly Apple _apple;

        private SnakeItem SnakeHead => _snakeParts[0];

        internal Snake(Apple apple)
        {
            _snakeParts = new Dictionary<int, SnakeItem>();
            _apple = apple;
        }

        internal void MakeStep(ref int x, ref int y, ConsoleKey? key)
        {
            if (key.HasValue)
            {
                SnakeHead.PreviousPosition = new Position(x, y);
                switch (key.Value)
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

        internal bool CheckPositionAndConfirmStep(int x, int y, char[,] map)
        {

            switch (map[y, x])
            {
                case Constant.MapBorderDesignation:
                case Constant.SnakeDesignation 
                when y != SnakeHead.CurrentPosition.X && x != SnakeHead.CurrentPosition.Y:
                    Console.Clear();
                    _snakeParts.Clear();
                    Console.WriteLine("Вы проиграли!");
                    return false;
                case Constant.AppleDesignation:
                    AddNewSnakePart(x, y);
                    map[y, x] = Constant.EmptyCell;
                    _apple.CreateApple(map);
                    break;
            }

            UpdateSnakePositions(x, y, map);

            return true;
        }

        private void UpdateSnakePositions(int x, int y, char[,] map)
        {
            for (var i = _snakeParts.Count - 1; i > 0; i--)
            {
                _snakeParts[i].PreviousPosition = _snakeParts[i].CurrentPosition;
                _snakeParts[i].CurrentPosition = _snakeParts[i - 1].CurrentPosition;
            }

            SnakeHead.CurrentPosition = new Position(x, y);

            foreach (var part in _snakeParts.Values)
            {
                map[part.CurrentPosition.Y, part.CurrentPosition.X] = Constant.SnakeDesignation;
                map[part.PreviousPosition.Y, part.PreviousPosition.X] = Constant.EmptyCell;
            }
        }

        internal void AddNewSnakePart(int x, int y)
        {
            var newItem = new SnakeItem(_snakeParts.LastOrDefault().Value?.PreviousPosition ?? new Position(x, y), new Position(x, y));
            _snakeParts.Add(_snakeParts.Count, newItem);
        }
    }

    internal class SnakeItem
    {
        internal Position CurrentPosition;
        internal Position PreviousPosition;
        public SnakeItem(Position currentPosition, Position previousPosition)
        {
            CurrentPosition = currentPosition;
            PreviousPosition = previousPosition;
        }
    }

    internal record struct Position(int X, int Y);
}
