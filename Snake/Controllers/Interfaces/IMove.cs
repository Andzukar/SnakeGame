namespace SnakeGame.Controllers.Interfaces;

/// <summary>
/// Movable abstraction.
/// </summary>
internal interface IMove
{
    /// <summary>
    /// Make step on map.
    /// </summary>
    /// <param name="x">Point x.</param>
    /// <param name="y">Point y.</param>
    void MakeStep(ref Position position);
}
