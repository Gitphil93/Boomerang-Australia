using System;
using System.IO;
using System.Collections.Generic;
using Boomerang.Data;
using Boomerang.Domain;
using Boomerang.Game;
using Boomerang.IO;

namespace Boomerang;

public static class Program
{
    public static void Main(string[] args)
    {
        var baseDir = AppContext.BaseDirectory;

        ICardRepository cardRepo = new JsonCardRepository(baseDir);
        IDeckService deckService = new DeckService(cardRepo);
        IScoringRules scoringRules = new AustraliaScoringRules();
        IPassDirectionStrategy passDirection = new FixedLeftPassDirection();
        IGamePresenter presenter = new ConsoleGamePresenter();
        IGameSetup setup = new ConsoleGameSetup();
        var config = new GameConfig();
        var (players, clients) = setup.ConfigurePlayers();

        var engine = new GameEngine(players, clients, deckService, scoringRules, passDirection, presenter, config);
        engine.RunGame();

        //Ta bort dessa?
       // Console.WriteLine();
       //Console.WriteLine("Game finished. Press any key to exit.");
       //Console.ReadKey();
    }
}
