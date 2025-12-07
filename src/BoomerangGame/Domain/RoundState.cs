using System.Collections.Generic;

namespace Boomerang.Domain;

/// <summary>
/// Data needed to score one round.
/// </summary>

public class RoundState
{
    public Player Player { get; }
    public List<Card> PlayedCards { get; }
    public List<Card> AllPlayerCards { get; }
    public int RoundIndex { get; }

    public RoundState(Player player, List<Card> playedCards, List<Card> allPlayerCards, int roundIndex)
    {
        Player = player;
        PlayedCards = playedCards;
        AllPlayerCards = allPlayerCards;
        RoundIndex = roundIndex;
    }
}
