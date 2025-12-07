namespace Boomerang.Game;


/// <summary>
/// Defines how hands are passed between players.
/// </summary>

public interface IPassDirectionStrategy
{
    int GetNextPlayerIndex(int currentIndex, int roundIndex, int playerCount);
}
