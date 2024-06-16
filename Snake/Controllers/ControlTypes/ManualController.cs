using SnakeGame.Controllers.Interfaces;

namespace SnakeGame.Controllers.ControlTypes;

/// <summary>
/// Manual controller.
/// </summary>
internal class ManualController : IMove
{
    /// <summary>
    /// User key input.
    /// </summary>
    private ConsoleKey? _key = null;

    /// <summary>
    /// Read user input.
    /// </summary>
    private void ReadKeyFromKeybourd()
    {
        Task.Run(() =>
        {
            while (true)
            {
                _key = Console.ReadKey().Key;
            }
        });
    }

    /// <inheritdoc/>
    public void MakeStep(ref Position snakeHeadPosition)
    {
        ReadKeyFromKeybourd();

        if (_key.HasValue)
        {
            switch (_key.Value)
            {
                case ConsoleKey.LeftArrow:
                    snakeHeadPosition.X--;
                    break;
                case ConsoleKey.RightArrow:
                    snakeHeadPosition.X++;
                    break;
                case ConsoleKey.UpArrow:
                    snakeHeadPosition.Y--;
                    break;
                case ConsoleKey.DownArrow:
                    snakeHeadPosition.Y++;
                    break;
            }
        }
    }
}
