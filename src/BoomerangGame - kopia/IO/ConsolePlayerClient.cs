using System;
using System.Collections.Generic;
using System.Linq;
using Boomerang.Domain;

namespace Boomerang.IO;

public class ConsolePlayerClient : IPlayerClient
{
    private readonly Player _player;

    public ConsolePlayerClient(Player player)
    {
        _player = player;
    }

    public Card ChooseThrowCard(PlayerViewState state)
    {
        Console.WriteLine();
        Console.WriteLine($"{_player.Name}, choose a card to THROW (this will be hidden until end of round):");
        PrintHand(state.Hand);

        int index = AskCardIndex(state.Hand.Count);
        return state.Hand[index];
    }

    // Just nu används inte denna i flödet (catch bestäms av sista kortet),
    // men vi behåller den för framtida varianter.
    public Card ChooseCatchCard(PlayerViewState state)
    {
        return state.Hand.First();
    }

    public void ShowRoundResult(RoundScore result)
    {
        Console.WriteLine();
        Console.WriteLine($"----- Round result for {_player.Name} -----");

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

        Console.WriteLine("\n----- Round Summary -----");
        Console.WriteLine($"Throw & Catch : {result.ThrowCatch}");
        Console.WriteLine($"Regions       : {result.Regions}");
        Console.WriteLine($"Collections   : {result.Collections}");
        Console.WriteLine($"Animals       : {result.Animals}");
        Console.WriteLine($"Activities    : {result.Activities}");
        Console.WriteLine($"TOTAL         : {result.Total}");
    }

    private static void PrintHand(IReadOnlyList<Card> hand)
    {
        Console.WriteLine("Your hand:");
        for (int i = 0; i < hand.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {FormatCard(hand[i])}");
        }
    }

    private static int AskCardIndex(int handSize)
    {
        while (true)
        {
            Console.Write("Select card number: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out var choice))
            {
                int idx = choice - 1;
                if (idx >= 0 && idx < handSize)
                    return idx;
            }

            Console.WriteLine("Invalid choice, try again.");
        }
    }

    private static string FormatCard(Card c)
    {
        
        return $"{c.Name} ({c.Region} {c.SiteLetter}) T:{c.ThrowValue} C:{c.CatchValue} " +
               $"[{c.CollectionType}] [{c.AnimalType}] [{c.ActivityType}]";
    }
}
