using Boomerang.Domain;

namespace Boomerang.IO;

/// <summary>
/// Handles player interaction for choosing cards and viewing results.
/// </summary>

public interface IPlayerClient
{
    Card ChooseThrowCard(PlayerViewState state);
    Card ChooseCatchCard(PlayerViewState state);
    void ShowRoundResult(RoundScore result);
}
