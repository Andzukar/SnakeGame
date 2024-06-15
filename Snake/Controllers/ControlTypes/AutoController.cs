using SnakeGame.Controllers.Interfaces;
using SnakeGame.Static;

namespace SnakeGame.Controllers.ControlTypes;

/// <summary>
/// Auto controller.
/// </summary>
internal class AutoController : IMove
{
    /// <summary>
    /// Game map.
    /// </summary>
    private readonly Map _map;

    /// <summary>
    /// Initialize object type of <see cref="AutoController"/>
    /// </summary>
    /// <param name="map">Game map.</param>
    public AutoController(Map map)
    {
        _map = map;
    }

    /// <inheritdoc/>
    public void MakeStep(ref int x, ref int y)
    {
        var nextPosition = GetNextPosition(new Position(x, y), GetApplePosition());

        x = nextPosition.X;
        y = nextPosition.Y;
    }

    /// <summary>
    /// Get next free position for snake.
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="applePosition"></param>
    /// <returns>Next position for snake.</returns>
    private Position GetNextPosition(Position currentPosition, Position applePosition)
    {
        var heuristicDistance = new Dictionary<Position, int>();

        var neigboursPositions = GetEmptyNeigboursPositions(currentPosition).ToArray();

        if (neigboursPositions.Length is 0)
        {
            return currentPosition;
        }

        foreach (var neigbourPosition in neigboursPositions)
        {
            var heuristicCost = Math.Abs(neigbourPosition.X - applePosition.X) + Math.Abs(neigbourPosition.Y - applePosition.Y);

            heuristicDistance.Add(neigbourPosition, heuristicCost);
        }

        var nextPosition = heuristicDistance.Aggregate((neigbour1, neigbour2) => neigbour1.Value < neigbour2.Value ? neigbour1 : neigbour2).Key;

        return nextPosition;
    }

    /// <summary>
    /// Get free neigbours positions.
    /// </summary>
    /// <param name="position">Position for which neighbors are sought.</param>
    /// <returns>Free neigbours positions</returns>
    private IEnumerable<Position> GetEmptyNeigboursPositions(Position position)
    {
        var neigbours = new Position[]
        {
            new(position.X, position.Y + 1),
            new(position.X, position.Y - 1),
            new(position.X + 1, position.Y),
            new(position.X - 1, position.Y)
        }.Where(neigbour => _map[neigbour.Y, neigbour.X] != Constant.MapBorderDesignation && _map[neigbour.Y, neigbour.X] != Constant.SnakeDesignation);

        return neigbours;
    }

    /// <summary>
    /// Get current apple position.
    /// </summary>
    /// <returns>Current apple position.</returns>
    private static Position GetApplePosition() => AppleManager.CurrentApplePosition;
}
