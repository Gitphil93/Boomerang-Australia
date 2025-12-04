namespace Boomerang.Game;

public class FixedLeftPassDirection : IPassDirectionStrategy
{
    public int GetNextPlayerIndex(int currentIndex, int roundIndex, int playerCount)
    {
        return (currentIndex + 1) % playerCount;
    }
}
