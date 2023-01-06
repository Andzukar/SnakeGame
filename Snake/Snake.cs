using Snake;

namespace SnakeGame
{
    internal class Snake
    {
        internal event Action ScoreUp;
        internal event Action<string> GameOver;
        internal SnakeItem SnakeHead => _snakeParts[0];

        private Dictionary<int, SnakeItem> _snakeParts;
        private readonly Apple _apple;


        internal Snake(Apple apple)
        {
            _snakeParts = new Dictionary<int, SnakeItem>();
            _apple = apple;
        }

        internal bool CheckPositionAndConfirmStep(int x, int y, char[,] map)
        {

            switch (map[y, x])
            {
                case Constant.MapBorderDesignation:
                    GameOver?.Invoke("You hit the wall!");
                    return false;
                case Constant.SnakeDesignation 
                when y != SnakeHead.CurrentPosition.X && x != SnakeHead.CurrentPosition.Y:
                    GameOver?.Invoke("You ate yourself!");
                    return false;
                case Constant.AppleDesignation:
                    AddNewSnakePart(x, y);
                    map[y, x] = Constant.EmptyCell;
                    ScoreUp?.Invoke();
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
