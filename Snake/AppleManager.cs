using Constant = SnakeGame.Static.Constant;

namespace SnakeGame;

/// <summary>
/// Apple manager.
/// </summary>
internal class AppleManager
{
    /// <summary>
    /// Current apple position.
    /// </summary>
    internal static Position CurrentApplePosition { get; private set; }

    /// <summary>
    /// Randomizer.
    /// </summary>
    private readonly Random _random;

    /// <summary>
    /// Initialize object type of <see cref="AppleManager"/>
    /// </summary>
    internal AppleManager()
    {
        _random = new Random();
    }

    /// <summary>
    /// Create apple on map.
    /// </summary>
    /// <param name="map">Game map.</param>
    internal void CreateApple(Map map)
    {
        var emptyCells = GetEmptyPositions(map);

        var randomEmptyCell = emptyCells.OrderBy(x => _random.Next()).First();

        map[randomEmptyCell.Y, randomEmptyCell.X] = Constant.AppleDesignation;
        CurrentApplePosition = randomEmptyCell;
    }

    /// <summary>
    /// Get empty positions for apple.
    /// </summary>
    /// <param name="map">Game map.</param>
    /// <returns>Empty positions.</returns>
    private IEnumerable<Position> GetEmptyPositions(Map map)
    {
        for (var i = 0; i < map.SizeY; i++)
        {
            for (var j = 0; j < map.SizeX; j++)
            {
                if (map[i, j] is Constant.EmptyPositionDesignation)
                {
                    var position = new Position(j, i);

                    if (position != CurrentApplePosition)
                    {
                        yield return position;
                    }
                }
            }
        }
    }

}
