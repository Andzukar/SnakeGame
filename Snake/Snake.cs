using SnakeGame.Static;

namespace SnakeGame;

/// <summary>
/// Snake.
/// </summary>
internal class Snake
{
    /// <summary>
    /// ScoreUp event.
    /// </summary>
    internal event Action? ScoreUp;

    /// <summary>
    /// GameOver event.
    /// </summary>
    internal event Action<string>? GameOver;

    /// <summary>
    /// Snake head.
    /// </summary>
    internal SnakePart SnakeHead => _snakeParts[0];

    /// <summary>
    /// Snake parts.
    /// </summary>
    private Dictionary<int, SnakePart> _snakeParts;

    /// <summary>
    /// Initialize object type of <see cref="Snake"/>
    /// </summary>
    internal Snake()
    {
        _snakeParts = new Dictionary<int, SnakePart>();
    }

    /// <summary>
    /// Confirm step for snake.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    /// <param name="map">Game map.</param>
    /// <returns>True - confirm is success, game is continue. False - game over.</returns>
    internal bool ConfirmStep(int x, int y, Map map)
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
                AddPart(x, y);
                map[y, x] = Constant.EmptyPositionDesignation;
                ScoreUp?.Invoke();
                break;
        }

        UpdateSnakePositions(x, y, map);

        return true;
    }

    /// <summary>
    /// Update snake positions.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    /// <param name="map">Game map.</param>
    private void UpdateSnakePositions(int x, int y, Map map)
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
            map[part.PreviousPosition.Y, part.PreviousPosition.X] = Constant.EmptyPositionDesignation;
        }
    }

    /// <summary>
    /// Add part to snake.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    internal void AddPart(int x, int y)
    {
        var newItem = new SnakePart(_snakeParts.LastOrDefault().Value?.PreviousPosition ?? new Position(x, y), new Position(x, y));
        _snakeParts.Add(_snakeParts.Count, newItem);
    }
}

/// <summary>
/// Snake part.
/// </summary>
internal class SnakePart
{
    internal Position CurrentPosition;
    internal Position PreviousPosition;
    public SnakePart(Position currentPosition, Position previousPosition)
    {
        CurrentPosition = currentPosition;
        PreviousPosition = previousPosition;
    }
}