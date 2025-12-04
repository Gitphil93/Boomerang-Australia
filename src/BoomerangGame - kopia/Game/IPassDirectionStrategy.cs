namespace Boomerang.Game;

public interface IPassDirectionStrategy
{
    int GetNextPlayerIndex(int currentIndex, int roundIndex, int playerCount);
}
