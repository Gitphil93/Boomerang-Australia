using Boomerang.Domain;

namespace Boomerang.IO;

public interface IPlayerClient
{
    Card ChooseThrowCard(PlayerViewState state);
    Card ChooseCatchCard(PlayerViewState state);
    void ShowRoundResult(RoundScore result);
}
