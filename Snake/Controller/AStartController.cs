using Snake;
using SnakeGame.Interface;
namespace SnakeGame.Controller
{
    internal class AStartController : IMove
    {
        public void MakeStep(ref int x, ref int y)
        {
            var nextCell = NextCell(new Position(x, y), GetApplePosition());

            if (nextCell is null)
            {
                return;
            }

            x = nextCell.Value.X;
            y = nextCell.Value.Y;
        }

        private Position? NextCell(Position startPosition, Position endPosition)
        {
            var heuristicDistance = new Dictionary<Position, int>();
            var openList = new List<Position>();
            var closedList = new List<Position>();

            var currentCell = startPosition;
            openList.Add(currentCell);
            closedList.Add(currentCell);
            var neigbours = GetNeigbours(currentCell);

            foreach (var neigbour in neigbours)
            {
                openList.Add(neigbour);

                if (closedList.Contains(neigbour))
                {
                    continue;
                }

                var cost = Math.Abs(neigbour.X - endPosition.X) + Math.Abs(neigbour.Y - endPosition.Y);
                if (!heuristicDistance.TryAdd(neigbour, cost))
                {
                    heuristicDistance[neigbour] = cost;
                }
            }

            return currentCell = openList.Except(closedList)
                .FirstOrDefault(heuristicDistance
                .Aggregate((item1, item2) => item1.Value < item2.Value ? item1 : item2).Key);
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
