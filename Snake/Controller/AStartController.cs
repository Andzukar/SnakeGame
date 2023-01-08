using Snake;
using SnakeGame.Interface;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Numerics;

namespace SnakeGame.Controller
{
    internal class AStartController : IMove
    {

        public void MakeStep(ref int x, ref int y)
        {
            var nextCell = GetNextCell(new Position(x, y), GetApplePosition());

            if (nextCell.HasValue)
            {
                x = nextCell.Value.X;
                y = nextCell.Value.Y;
            }
        }

        private static Position? GetNextCell(Position currentPosition, Position applePosition)
        {
            var heuristicDistance = new Dictionary<Position, int>();
            var path = new Dictionary<Position, Position>();
            var openList = new List<Position>();
            var closedList = new List<Position>();
            var startPosition = currentPosition;

            openList.Add(currentPosition);
            while (openList.Count is not 0)
            {
                openList.Remove(currentPosition);
                closedList.Add(currentPosition);

                var neigbours = GetNeigbours(currentPosition);

                foreach (var neigbour in neigbours.Except(closedList))
                {
                    if (!openList.Contains(neigbour))
                    {
                        openList.Add(neigbour);
                    }

                    if (!path.TryAdd(neigbour, currentPosition))
                    {
                        path[neigbour] = currentPosition;
                    }

                    var heuristicCost = Math.Abs(neigbour.X - applePosition.X) + Math.Abs(neigbour.Y - applePosition.Y);

                    if (!heuristicDistance.TryAdd(neigbour, heuristicCost))
                    {
                        heuristicDistance[neigbour] = heuristicCost;
                    }
                }

                currentPosition = GetPositionWithMinCost(openList, heuristicDistance);

                if (currentPosition == applePosition)
                {
                    return GetParentOfCurrentCell(path, currentPosition, startPosition);
                }

            }

            return null;
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
                 || cellFillCharacter is Constant.SnakeDesignation)
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
