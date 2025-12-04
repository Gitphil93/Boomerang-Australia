using System;
using System.Collections.Generic;
using System.Linq;
using Boomerang.Domain;

namespace Boomerang.Game;

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

        // Total är read-only, räknas automatiskt i RoundScore
        return new RoundScore
        {
            ThrowCatch = throwCatch,
            Regions = regions,
            Collections = collections,
            Animals = animals,
            Activities = activities
        };
    }

    // 10a) Throw & Catch = |Throw - Catch|
    private static int CalculateThrowCatch(IReadOnlyList<Card> cards)
    {
        var throwCard = cards[0];
        var catchCard = cards[^1];

        // Antag att JSON:ens "number" mappas till ThrowValue (och ev. även CatchValue)
        return Math.Abs(throwCard.ThrowValue - catchCard.ThrowValue);
    }

    // 10b) Tourist sites + enkel regionbonus
    private static int CalculateRegions(Player player, IReadOnlyList<Card> cards)
    {
        int siteScore = 0;

        foreach (var card in cards)
        {
            if (!string.IsNullOrWhiteSpace(card.SiteLetter))
            {
                // +1 första gången spelaren ser denna site under hela spelet
                if (player.VisitedSites.Add(card.SiteLetter))
                {
                    siteScore++;
                }
            }
        }

        // Enkel regionbonus: om en spelare i denna runda har minst 4 olika sites
        // i samma region och inte redan fått bonus för den regionen → +3
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

    // 10c) Collections: Leaves=1, Wildflowers=2, Shells=3, Souvenirs=5
    // Summa 1–7 → dubbla, >7 → bara summan
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

    // 10d) Animals: poäng per PAR
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

    // 10e) Activities: välj bästa aktivitet denna runda,
    // score: 1→0, 2→2, 3→4, 4→7, 5→10, 6→15. Varje aktivitet max en gång per game.
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

            // får bara score:a en aktivitet en gång per game
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
