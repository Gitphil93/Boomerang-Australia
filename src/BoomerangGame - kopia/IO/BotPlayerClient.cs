using System;
using System.Linq;
using Boomerang.Domain;

namespace Boomerang.IO;

public class BotPlayerClient : IPlayerClient
{
    private readonly Player _player;
    private readonly Random _rng = new();

    public BotPlayerClient(Player player)
    {
        _player = player;
    }

    public Card ChooseThrowCard(PlayerViewState state)
    {
        //  välj det kort med högst ThrowValue
        var best = state.Hand
            .OrderByDescending(c => c.ThrowValue)
            .First();

        Console.WriteLine();
        Console.WriteLine($"{_player.Name} (bot) is choosing a Throw card...");
        System.Threading.Thread.Sleep(1000);
        // Vi visar inte vilket kort innan rundan är slut

        return best;
    }

    public Card ChooseCatchCard(PlayerViewState state)
    {
        // Används egentligen inte, styrs av sista kortet i GameEngine
        // men vi returnerar bara första kortet för säkerhets skull
        return state.Hand.First();
    }

    public void ShowRoundResult(RoundScore result)
    {
        Console.WriteLine();
        Console.WriteLine($"--- Round result for bot {_player.Name} ---");

        if (_player.PlayedCards.Count == 7)
        {
            var throwCard = _player.PlayedCards[0];
            var catchCard = _player.PlayedCards[6];
            var drafted = _player.PlayedCards.Skip(1).Take(5).ToList();

            Console.WriteLine($"Throw card : {FormatCard(throwCard)}");
            Console.WriteLine($"Catch card : {FormatCard(catchCard)}");
            Console.WriteLine("Drafted cards:");
            foreach (var c in drafted)
            {
                Console.WriteLine($"  - {FormatCard(c)}");
            }
        }
        else
        {
            Console.WriteLine("(Unexpected number of played cards, cannot show card details.)");
        }

        Console.WriteLine();
        Console.WriteLine("----- Round Summary -----");
        Console.WriteLine($"Throw & Catch : {result.ThrowCatch}");
        Console.WriteLine($"Regions       : {result.Regions}");
        Console.WriteLine($"Collections   : {result.Collections}");
        Console.WriteLine($"Animals       : {result.Animals}");
        Console.WriteLine($"Activities    : {result.Activities}");
        Console.WriteLine($"TOTAL         : {result.Total}");
    }

    private static string FormatCard(Card c)
    {
        return $"{c.Name} ({c.Region} {c.SiteLetter}) T:{c.ThrowValue} C:{c.CatchValue} " +
               $"[{c.CollectionType}] [{c.AnimalType}] [{c.ActivityType}]";
    }
}
