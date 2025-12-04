using System;
using System.Collections.Generic;
using Boomerang.Domain;

namespace Boomerang.Game;

public class DeckService : IDeckService
{
    private readonly List<Card> _allCards;

    public DeckService(string jsonPath)
    {
        _allCards = CardRepository.LoadFromJson(jsonPath);
    }

    public List<Card> CreateShuffledDeck()
    {
        var deck = new List<Card>(_allCards);
        var rng = new Random();
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
        return deck;
    }
}
