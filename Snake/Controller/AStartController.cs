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
                var cost = Math.Abs(neigbour.X - applePosition.X) + Math.Abs(neigbour.Y - applePosition.Y);
                heuristicDistance.Add(neigbour, cost);
            }

            var maxCost = float.MinValue;
            Position? nextPosition = null;
            foreach (var neigbour in heuristicDistance)
            {
                if (neigbour.Value > maxCost)
                {
                    maxCost = neigbour.Value;
                    nextPosition = neigbour.Key;
                }
            }
            if (nextPosition.HasValue)
            {
                _closedList.Add(nextPosition.Value);
            }

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

            foreach (var neigbour in allPossibleNeigbours.ToList())
            {
                if (neigbour.X < 0 || neigbour.Y < 0)
                {
                    allPossibleNeigbours.Remove(neigbour);
                    continue;
                }

                var cellFillCharacter = Map.GetCurrentMap[neigbour.Y, neigbour.X];
                if (cellFillCharacter is Constant.MapBorderDesignation
                 || cellFillCharacter is Constant.SnakeDesignation)
                {
                    allPossibleNeigbours.Remove(neigbour);
                }
            }

            return allPossibleNeigbours;
        }

        private static Position GetApplePosition() => Apple.CurrentApplePosition;

    }
}
