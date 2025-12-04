using System;
using System.Collections.Generic;
using System.IO;
using Boomerang.Domain;
using Boomerang.Game;
using Boomerang.IO;

namespace BoomerangGame;

public static class Program
{
    public static void Main(string[] args)
    {
        // Hitta json
        var baseDir = AppContext.BaseDirectory;
        var cardsPath = Path.Combine(baseDir, "Data", "cards_australia.json");

        IDeckService deckService = new DeckService(cardsPath);
        IScoringRules scoringRules = new AustraliaScoringRules();
        IPassDirectionStrategy passDirection = new FixedLeftPassDirection();


        //Lite spelsetup innan GameEngine kör. Borde jag lösa i IO istället? Nja, isf i typ ConsoleGameSetup & IGameSetup
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
            {
                name = $"Player {i}";
            }

            var player = new Player(name);
            players.Add(player);

            // FRÅGA: Är denna spelare en bot?
            Console.Write($"Should {name} be a bot? (y/n): ");
            var botAnswer = Console.ReadLine()?.Trim().ToLower();

            if (botAnswer == "y" || botAnswer == "yes")
            {
                clients.Add(new BotPlayerClient(player));
            }
            else
            {
                clients.Add(new ConsolePlayerClient(player));
            }
        }


        var presenter = new ConsoleGameResultsPresenter();

        var engine = new GameEngine(players, clients.ToArray(), deckService, scoringRules, passDirection, presenter);
        engine.RunGame();

        Console.WriteLine();
        Console.WriteLine("Game finished. Press any key to exit.");
        Console.ReadKey();
    }

    private static int AskPlayerCount()
    {
        while (true)
        {
            Console.Write("How many players (2-4)? ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out var count) && count is >= 2 and <= 4)
            {
                return count;
            }

            Console.WriteLine("Invalid number of players. Please enter a number between 2 and 4.");
        }
    }
}
