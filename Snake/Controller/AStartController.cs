using Snake;
using SnakeGame.Interface;
namespace SnakeGame.Controller
{
    internal class AStartController : IMove
    {
        public void MakeStep(ref int x, ref int y)
        {
            var nextCell = NextCell(new Position(x, y), GetApplePosition());

            if (nextCell.HasValue)
            {
                x = nextCell.Value.X;
                y = nextCell.Value.Y;
            }

        }

        private Position? NextCell(Position startPosition, Position endPosition)
        {
            var heuristicDistance = new Dictionary<Position, int>();

            var neigbours = GetNeigbours(startPosition);

            foreach (var neigbour in neigbours)
            {
                var cost = Math.Abs(neigbour.X - endPosition.X) + Math.Abs(neigbour.Y - endPosition.Y);
                heuristicDistance[neigbour] = cost;
            }

            var minCost = int.MaxValue;
            Position? nextPosition = null;
            foreach(var neigbour in heuristicDistance)
            {
                if (neigbour.Value < minCost)
                {
                    minCost = neigbour.Value;
                    nextPosition = neigbour.Key;
                }
            }
            return nextPosition;
        }

        private List<Position> GetNeigbours(Position cell)
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
                var cellFillCharacter = Map.GetCurrentMap[n.X, n.Y];
                if (cellFillCharacter is Constant.MapBorderDesignation
                 || cellFillCharacter is Constant.SnakeDesignation)
                {
                    blockedPath.Add(n);
                }
            }
            return allPossibleNeigbours.Except(blockedPath).ToList();
        }

        private Position GetApplePosition() => Apple.CurrentApplePosition;

    }
}
