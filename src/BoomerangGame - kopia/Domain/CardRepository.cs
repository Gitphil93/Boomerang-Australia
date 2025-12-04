using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Boomerang.Domain;

public static class CardRepository
{
    public static List<Card> LoadFromJson(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Card JSON not found at {path}");

        var json = File.ReadAllText(path);

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
