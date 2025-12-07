namespace Boomerang.Game;

/// <summary>
/// Passes hands to the left each round.
/// </summary>

public class FixedLeftPassDirection : IPassDirectionStrategy
{
    public int GetNextPlayerIndex(int currentIndex, int roundIndex, int playerCount)
    {
        return (currentIndex + 1) % playerCount;
    }
}
