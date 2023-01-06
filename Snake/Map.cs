using Snake;

namespace SnakeGame
{
    internal class Map
    {
        internal static char[,] GetCurrentMap => map;

        private static char[,]? map;

        internal static char[,] GenerateMap(int size)
        {
            map = new char[size, size * 2];
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = ' ';
                    map[i, map.GetLength(1) - 1] = Constant.MapBorderDesignation;
                    map[map.GetLength(0) - 1, j] = Constant.MapBorderDesignation;
                    map[0, j] = Constant.MapBorderDesignation;
                    map[i, 0] = Constant.MapBorderDesignation;
                }
            }
            return map;
        }

        internal static void DrawMap(char[,] map)
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i, j])
                    {
                        case Constant.SnakeDesignation:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case Constant.MapBorderDesignation:
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case Constant.AppleDesignation:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                    }

                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }

    }
}
