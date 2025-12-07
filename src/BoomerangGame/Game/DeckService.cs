using System;
using System.Collections.Generic;
using Boomerang.Domain;

namespace Boomerang.Game;

/// <summary>
/// Loads, creates and shuffles the deck.
/// </summary>

public class DeckService : IDeckService
{
    private readonly ICardRepository _repo;

    public DeckService(ICardRepository repo)
    {
        _repo = repo;
    }

    public List<Card> CreateShuffledDeck()
    {
        var deck = _repo.LoadAll();
        var rng = new Random();

        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }
}
