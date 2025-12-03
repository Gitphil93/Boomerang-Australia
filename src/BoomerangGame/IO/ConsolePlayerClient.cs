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
        // TODO: Implementera riktigt val
        return state.Hand.First();
    }

    public Card ChooseCatchCard(PlayerViewState state)
    {
        // TODO: Implementera riktigt val
        return state.Hand.First();
    }

    public void ShowRoundResult(RoundScore result)
    {
        Console.WriteLine($"{_player.Name} scored {result.Total} points this round.");
    }
}
