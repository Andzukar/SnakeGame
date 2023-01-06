using Snake;
using SnakeGame.Interface;
using System.Numerics;

namespace SnakeGame.Controller
{
    internal class AStartController : IMove
    {
        internal readonly static List<Position> _closedList = new();

        public void MakeStep(ref int x, ref int y)
        {
            var nextCell = NextCell(new Position(x, y), GetApplePosition());

            if (nextCell.HasValue)
            {
                x = nextCell.Value.X;
                y = nextCell.Value.Y;
            }
        }

        private static Position? NextCell(Position currentPosition, Position applePosition)
        {
            var heuristicDistance = new Dictionary<Position, float>();

            var neigbours = GetNeigbours(currentPosition);
            _closedList.Add(currentPosition);
            foreach (var neigbour in neigbours.Except(_closedList))
            {
                var cost = Vector2.Distance(new Vector2(neigbour.X, neigbour.Y), new Vector2(applePosition.X, applePosition.Y));
                heuristicDistance.Add(neigbour, cost);
            }

            var minCost = float.MinValue;
            var nextPosition = new Position(0, 0);
            foreach (var neigbour in heuristicDistance)
            {
                if (neigbour.Value > minCost)
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
            foreach (var neigbour in allPossibleNeigbours)
            {
                var cellFillCharacter = Map.GetCurrentMap[neigbour.Y, neigbour.X];
                if (cellFillCharacter is Constant.MapBorderDesignation
                 || cellFillCharacter is Constant.SnakeDesignation)
                {
                    blockedPath.Add(neigbour);
                }
            }
            return allPossibleNeigbours.Except(blockedPath).ToList();
        }

        private static Position GetApplePosition() => Apple.CurrentApplePosition;

    }
}
