using System;
using System.Collections.Generic;
using System.Linq;
using Boomerang.Domain;

namespace Boomerang.Game;

/// <summary>
/// Scoring rules for the Australia edition.
/// </summary>

public class AustraliaScoringRules : IScoringRules
{
    public RoundScore CalculateRoundScore(RoundState state)
    {
        if (state == null) throw new ArgumentNullException(nameof(state));
        var cards = state.PlayedCards ?? throw new ArgumentNullException(nameof(state.PlayedCards));

        if (cards.Count != 7)
            throw new InvalidOperationException("Round must contain exactly 7 cards.");

        var player = state.Player;

        int throwCatch = CalculateThrowCatch(cards);
        int regions = CalculateRegions(player, cards);
        int collections = CalculateCollections(cards);
        int animals = CalculateAnimals(cards);
        int activities = CalculateActivities(player, cards);

        return new RoundScore
        {
            ThrowCatch = throwCatch,
            Regions = regions,
            Collections = collections,
            Animals = animals,
            Activities = activities
        };
    }

    private static int CalculateThrowCatch(IReadOnlyList<Card> cards)
    {
        var throwCard = cards[0];
        var catchCard = cards[^1];
       
        return Math.Abs(throwCard.ThrowValue - catchCard.ThrowValue);
    }


    private static int CalculateRegions(Player player, IReadOnlyList<Card> cards)
    {
        int siteScore = 0;

        foreach (var card in cards)
        {
            if (!string.IsNullOrWhiteSpace(card.SiteLetter))
            {

                if (player.VisitedSites.Add(card.SiteLetter))
                {
                    siteScore++;
                }
            }
        }

        int regionBonus = 0;

        var byRegionThisRound = cards
            .Where(c => !string.IsNullOrWhiteSpace(c.Region) && !string.IsNullOrWhiteSpace(c.SiteLetter))
            .GroupBy(c => c.Region);

        foreach (var group in byRegionThisRound)
        {
            string region = group.Key;
            if (player.CompletedRegions.Contains(region))
                continue;

            int distinctSites = group
                .Select(c => c.SiteLetter)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            if (distinctSites >= 4)
            {
                player.CompletedRegions.Add(region);
                regionBonus += 3;
            }
        }

        return siteScore + regionBonus;
    }

    private static int CalculateCollections(IReadOnlyList<Card> cards)
    {
        int total = 0;

        foreach (var card in cards)
        {
            switch (card.CollectionType)
            {
                case "Leaves":
                    total += 1;
                    break;
                case "Wildflowers":
                    total += 2;
                    break;
                case "Shells":
                    total += 3;
                    break;
                case "Souvenirs":
                    total += 5;
                    break;
            }
        }

        if (total == 0)
            return 0;

        return total <= 7 ? total * 2 : total;
    }

    private static int CalculateAnimals(IReadOnlyList<Card> cards)
    {
        var counts = cards
            .Where(c => !string.IsNullOrWhiteSpace(c.AnimalType))
            .GroupBy(c => c.AnimalType)
            .ToDictionary(g => g.Key, g => g.Count());

        int score = 0;

        foreach (var kvp in counts)
        {
            string animal = kvp.Key;
            int count = kvp.Value;

            int pairs = count / 2;
            if (pairs <= 0) continue;

            int perPair = animal switch
            {
                "Kangaroos" => 3,
                "Emus" => 4,
                "Wombats" => 5,
                "Koalas" => 7,
                "Platypuses" => 9,
                _ => 0
            };

            score += pairs * perPair;
        }

        return score;
    }

    private static int CalculateActivities(Player player, IReadOnlyList<Card> cards)
    {
        var counts = cards
            .Where(c => !string.IsNullOrWhiteSpace(c.ActivityType))
            .GroupBy(c => c.ActivityType)
            .ToDictionary(g => g.Key, g => g.Count());

        if (counts.Count == 0)
            return 0;

        string? bestActivity = null;
        int bestScore = 0;

        foreach (var kvp in counts)
        {
            string activity = kvp.Key;
            int count = kvp.Value;

            if (player.ScoredActivities.Contains(activity))
                continue;

            int score = count switch
            {
                1 => 0,
                2 => 2,
                3 => 4,
                4 => 7,
                5 => 10,
                6 => 15,
                _ => count > 6 ? 15 : 0
            };

            if (score > bestScore)
            {
                bestScore = score;
                bestActivity = activity;
            }
        }

        if (bestScore > 0 && bestActivity != null)
        {
            player.ScoredActivities.Add(bestActivity);
        }

        return bestScore;
    }
}
