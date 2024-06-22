using SnakeGame.Static;

namespace SnakeGame;

/// <summary>
/// Game map.
/// </summary>
internal class Map
{
    /// <summary>
    /// Size map by x.
    /// </summary>
    internal int SizeX => _map.GetLength(1);

    /// <summary>
    /// Size map by y.
    /// </summary>
    internal int SizeY => _map.GetLength(0);

    /// <summary>
    /// Game map.
    /// </summary>
    private char[,] _map;

    /// <summary>
    /// Indexer.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    /// <returns>Position value.</returns>
    internal char this[Position position]
    {
        get => _map[position.Y, position.X];
        set => _map[position.Y, position.X] = value;
    }

    /// <summary>
    /// Initialize object type of <see cref="Map"/>
    /// </summary>
    /// <param name="size">Map size.</param>
    internal Map(int size)
    {
        _map = new char[size, size * 2];
        GenerateMap();
    }

    /// <summary>
    /// Redraw map.
    /// </summary>
    internal void UpdateMap()
    {
        for (var i = 0; i < SizeY; i++)
        {
            for (var j = 0; j < SizeX; j++)
            {
                Console.SetCursorPosition(j, i);
                Console.Write(string.Empty);

                switch (_map[i, j])
                {
                    case Constant.SnakeDesignation:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case Constant.MapBorderDesignation:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case Constant.AppleDesignation:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }

                Console.Write(_map[i, j]);
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Generate map for game.
    /// </summary>
    private void GenerateMap()
    {
        for (var i = 0; i < SizeY; i++)
        {
            for (var j = 0; j < SizeX; j++)
            {
                _map[i, j] = Constant.EmptyPositionDesignation;
                _map[i, SizeX - 1] = Constant.MapBorderDesignation;
                _map[SizeY - 1, j] = Constant.MapBorderDesignation;
                _map[0, j] = Constant.MapBorderDesignation;
                _map[i, 0] = Constant.MapBorderDesignation;
            }
        }
    }

}
