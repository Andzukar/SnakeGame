using Snake;
using SnakeGame.Interface;
namespace SnakeGame.Controller
{
    internal class AStartController : IMove
    {
        internal static List<Position> _closedList = new();

        public void MakeStep(ref int x, ref int y)
        {
            var nextCell = NextCell(new Position(x, y), GetApplePosition());

            if (nextCell.HasValue)
            {
                x = nextCell.Value.X;
                y = nextCell.Value.Y;
            }
        }

        private static Position? NextCell(Position startPosition, Position endPosition)
        {
            var heuristicDistance = new Dictionary<Position, int>();

            var neigbours = GetNeigbours(startPosition);

            foreach (var neigbour in neigbours.Except(_closedList))
            {
                var cost = Math.Abs(neigbour.X - endPosition.X) + Math.Abs(neigbour.Y - endPosition.Y);
                heuristicDistance[neigbour] = cost;
            }

            var minCost = int.MaxValue;
            var nextPosition = startPosition;
            foreach (var neigbour in heuristicDistance)
            {
                if (neigbour.Value < minCost)
                {
                    minCost = neigbour.Value;
                    nextPosition = neigbour.Key;
                }
            }
            _closedList.Add(nextPosition);
            return nextPosition;
        }

        private static List<Position> GetNeigbours(Position cell)
        {
            var allPossibleNeigbours = new List<Position> {
                new Position(cell.X, cell.Y + 1),
                new Position(cell.X, cell.Y - 1),
                new Position(cell.X + 1, cell.Y),
                new Position(cell.X - 1, cell.Y)
            };

            var blockedPath = new List<Position>();
            foreach (var n in allPossibleNeigbours)
            {
                var cellFillCharacter = Map.GetCurrentMap[n.Y, n.X];
                if (cellFillCharacter is Constant.MapBorderDesignation
                 || cellFillCharacter is Constant.SnakeDesignation)
                {
                    blockedPath.Add(n);
                }
            }
            return allPossibleNeigbours.Except(blockedPath).ToList();
        }

        private static Position GetApplePosition() => Apple.CurrentApplePosition;

    }
}
