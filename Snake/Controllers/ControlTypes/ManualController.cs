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
    public void MakeStep(ref int x, ref int y)
    {
        ReadKeyFromKeybourd();

        if (_key.HasValue)
        {
            switch (_key.Value)
            {
                case ConsoleKey.LeftArrow:
                    x--;
                    break;
                case ConsoleKey.RightArrow:
                    x++;
                    break;
                case ConsoleKey.UpArrow:
                    y--;
                    break;
                case ConsoleKey.DownArrow:
                    y++;
                    break;
            }
        }
    }
}
