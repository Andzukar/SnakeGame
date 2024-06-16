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
    /// Move snake.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    /// <param name="map">Game map.</param>
    /// <returns>True - move is success, game is continue. False - game over.</returns>
    internal bool Move(Position newHeadPosition, Map map)
    {
        switch (map[newHeadPosition])
        {
            case Constant.MapBorderDesignation:
                GameOver?.Invoke("You hit the wall!");
                return false;
            case Constant.SnakeDesignation 
            when newHeadPosition.Y != SnakeHead.CurrentPosition.X && newHeadPosition.X != SnakeHead.CurrentPosition.Y:
                GameOver?.Invoke("You ate yourself!");
                return false;
            case Constant.AppleDesignation:
                AddPart(newHeadPosition);
                map[newHeadPosition] = Constant.EmptyPositionDesignation;
                ScoreUp?.Invoke();
                break;
        }

        UpdateSnakePositions(newHeadPosition, map);

        return true;
    }

    /// <summary>
    /// Update snake positions.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    /// <param name="map">Game map.</param>
    private void UpdateSnakePositions(Position newHeadPosition, Map map)
    {
        for (var i = _snakeParts.Count - 1; i > 0; i--)
        {
            _snakeParts[i].PreviousPosition = _snakeParts[i].CurrentPosition;
            _snakeParts[i].CurrentPosition = _snakeParts[i - 1].CurrentPosition;
        }

        SnakeHead.CurrentPosition = newHeadPosition;

        foreach (var part in _snakeParts.Values)
        {
            map[part.CurrentPosition] = Constant.SnakeDesignation;
            map[part.PreviousPosition] = Constant.EmptyPositionDesignation;
        }
    }

    /// <summary>
    /// Add part to snake.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    internal void AddPart(Position partPosition)
    {
        var newItem = new SnakePart(_snakeParts.LastOrDefault().Value?.PreviousPosition ?? partPosition, partPosition);
        _snakeParts.Add(_snakeParts.Count, newItem);
    }
}

/// <summary>
/// Snake part.
/// </summary>
internal class SnakePart
{
    /// <summary>
    /// Current snake part position.
    /// </summary>
    internal Position CurrentPosition;

    /// <summary>
    /// Previous snake part position.
    /// </summary>
    internal Position PreviousPosition;

    /// <summary>
    /// Initialize object type of <see cref="SnakePart"/>
    /// </summary>
    /// <param name="currentPosition">Current snake part position.</param>
    /// <param name="previousPosition">Previous snake part position.</param>
    internal SnakePart(Position currentPosition, Position previousPosition)
    {
        CurrentPosition = currentPosition;
        PreviousPosition = previousPosition;
    }
}