using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Boomerang.Domain;
using Boomerang.Game;

namespace Boomerang.Data;

public class JsonCardRepository : ICardRepository
{
    private readonly string _cardsPath;

    public JsonCardRepository(string baseDir)
    {
        _cardsPath = Path.Combine(baseDir, "Data", "cards_australia.json");
    }

    public List<Card> LoadAll()
    {
        if (!File.Exists(_cardsPath))
            throw new FileNotFoundException($"Card JSON not found at {_cardsPath}");

        var json = File.ReadAllText(_cardsPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var cards = JsonSerializer.Deserialize<List<Card>>(json, options);

        if (cards == null || cards.Count == 0)
            throw new Exception("No cards loaded from JSON.");

        return cards;
    }
}
