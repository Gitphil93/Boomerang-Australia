using System;
using Boomerang.Domain;
using Boomerang.Domain;

/// <summary>
/// Placeholder for future network-based player interaction.
/// </summary>

namespace Boomerang.IO;

public class NetworkPlayerClient : IPlayerClient
{
    public Card ChooseThrowCard(PlayerViewState state)
    {
        throw new NotImplementedException();
    }

    public Card ChooseCatchCard(PlayerViewState state)
    {
        throw new NotImplementedException();
    }

    public void ShowRoundResult(RoundScore result)
    {
        throw new NotImplementedException();
    }
}
