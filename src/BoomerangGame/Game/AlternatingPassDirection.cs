namespace Boomerang.Game;

/// <summary>
/// Alternates pass direction between rounds.
/// </summary>

public class AlternatingPassDirection : IPassDirectionStrategy
{
    public int GetNextPlayerIndex(int currentIndex, int roundIndex, int playerCount)
    {
        bool passLeft = roundIndex % 2 == 0;
        return passLeft
            ? (currentIndex + 1) % playerCount
            : (currentIndex - 1 + playerCount) % playerCount;
    }
}
