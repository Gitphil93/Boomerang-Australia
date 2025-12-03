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
        var baseDir = AppContext.BaseDirectory;
        var cardsPath = Path.Combine(baseDir, "Data", "cards_australia.json");

        var deckService = new DeckService(cardsPath);
        var scoringRules = new AustraliaScoringRules();
        var passDirection = new FixedLeftPassDirection();

        var players = new List<Player>
        {
            new Player("Player 1"),
            new Player("Player 2")
        };

        var clients = new IPlayerClient[]
        {
            new ConsolePlayerClient(players[0]),
            new ConsolePlayerClient(players[1])
        };

        var engine = new GameEngine(players, clients, deckService, scoringRules, passDirection);
        engine.RunGame();
    }
}
