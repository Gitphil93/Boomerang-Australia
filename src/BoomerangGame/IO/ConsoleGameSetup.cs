using Boomerang.Domain;
using Boomerang.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boomerang.IO;

public class ConsoleGameSetup : IGameSetup
{
    public (List<Player> players, IPlayerClient[] clients) ConfigurePlayers()
    {
        Console.WriteLine("Welcome to Boomerang: Australia!");
        Console.WriteLine();

        int playerCount = AskPlayerCount();

        var players = new List<Player>();
        var clients = new List<IPlayerClient>();

        for (int i = 1; i <= playerCount; i++)
        {
            Console.Write($"Enter name for player {i} (leave empty for 'Player {i}'): ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
                name = $"Player {i}";

            var player = new Player(name);
            players.Add(player);

            Console.Write($"Should {name} be a bot? (y/n): ");
            var botAnswer = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (botAnswer is "y" or "yes") { 
                clients.Add(new BotPlayerClient(player));
                Console.WriteLine("Training bot..");
            }
            else
                clients.Add(new ConsolePlayerClient(player));
        }

        return (players, clients.ToArray());
    }

    private static int AskPlayerCount()
    {
        while (true)
        {
            Console.Write("How many players (2-4)? ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out var count) && count is >= 2 and <= 4)
                return count;

            Console.WriteLine("Invalid number of players. Please enter a number between 2 and 4.");
        }
    }
}
