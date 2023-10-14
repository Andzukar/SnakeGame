using Snake;
using SnakeGame.Interface;

namespace SnakeGame.Controller
{
    internal class AStartController : IMove
    {
        public void MakeStep(ref int x, ref int y)
        {
            var nextPosition = GetNextPosition(new Position(x, y), GetApplePosition());

            x = nextPosition.X;
            y = nextPosition.Y;
        }

        private static Position GetNextPosition(Position currentPosition, Position applePosition)
        {
            var heuristicDistance = new Dictionary<Position, int>();
            var path = new Dictionary<Position, Position>();
            var openList = new List<Position>();
            var closedList = new List<Position>();
            var startPosition = currentPosition;

            do
            {
                openList.Remove(currentPosition);
                closedList.Add(currentPosition);

                var neigbours = GetNeigbours(currentPosition);

                foreach (var neigbour in neigbours.Except(closedList))
                {
                    openList.Add(neigbour);

                    path.AddOrUpdate(neigbour, currentPosition);

                    var heuristicCost = Math.Abs(neigbour.X - applePosition.X) + Math.Abs(neigbour.Y - applePosition.Y);

                    heuristicDistance.AddOrUpdate(neigbour, heuristicCost);
                }

                currentPosition = GetPositionWithMinCost(openList, heuristicDistance);

            } while (currentPosition != applePosition);

            return GetParentOfCurrentCell(path, currentPosition, startPosition);
        }

        private static List<Position> GetNeigbours(Position cell)
        {
            var allPossibleNeigbours = new List<Position> {
                new Position(cell.X, cell.Y + 1),
                new Position(cell.X, cell.Y - 1),
                new Position(cell.X + 1, cell.Y),
                new Position(cell.X - 1, cell.Y)
            };

            foreach (var neigbour in allPossibleNeigbours.ToList())
            {
                var cellFillCharacter = Map.GetCurrentMap[neigbour.Y, neigbour.X];
                if (cellFillCharacter is Constant.MapBorderDesignation
                 /*|| cellFillCharacter is Constant.SnakeDesignation*/)
                {
                    allPossibleNeigbours.Remove(neigbour);
                }
            }

            return allPossibleNeigbours;
        }

        private static Position GetParentOfCurrentCell(Dictionary<Position, Position> cellParents, Position currentPosition, Position startPosition)
        {
            var cell = cellParents.FirstOrDefault(c => c.Key == currentPosition);
            while (cell.Value != startPosition)
            {
                cell = cellParents.FirstOrDefault(c => c.Key == cell.Value);
            }
            return cell.Key;
        }

        private static Position GetPositionWithMinCost(List<Position> openList, Dictionary<Position, int> heuristicDistance) =>
                    openList.Select(o => heuristicDistance
                    .Aggregate((item1, item2) => item1.Value < item2.Value ? item1 : item2).Key)
                    .FirstOrDefault();

        private static Position GetApplePosition() => Apple.CurrentApplePosition;

    }
}
