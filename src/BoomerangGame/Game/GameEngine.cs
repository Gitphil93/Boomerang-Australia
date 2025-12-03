using System;
using System.Collections.Generic;
using System.Linq;
using Boomerang.Domain;
using Boomerang.IO;


namespace Boomerang.Game;

public class GameEngine
{
    private readonly List<Player> _players;
    private readonly IScoringRules _scoringRules;
    private readonly IPassDirectionStrategy _passDirection;
    private readonly IDeckService _deckService;
    private readonly IPlayerClient[] _clients;

    public GameEngine(
        List<Player> players,
        IPlayerClient[] clients,
        IDeckService deckService,
        IScoringRules scoringRules,
        IPassDirectionStrategy passDirection)
    {
        _players = players;
        _clients = clients;
        _deckService = deckService;
        _scoringRules = scoringRules;
        _passDirection = passDirection;
    }

    public void RunGame()
    {
        Console.WriteLine("Starting Boomerang game...");

        for (int round = 0; round < 1; round++) // börja med 1 runda för test
        {
            RunRound(round);
        }
    }

    public void RunRound(int roundIndex)
    {
        var deck = _deckService.CreateShuffledDeck();

        // Dela ut 7 kort per spelare
        const int cardsPerPlayer = 7;
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Hand.Clear();
            _players[i].Hand.AddRange(deck.Skip(i * cardsPerPlayer).Take(cardsPerPlayer));
        }

        // Låt varje spelare spela ett "throw" + "catch"
        for (int i = 0; i < _players.Count; i++)
        {
            var player = _players[i];
            var client = _clients[i];

            var view = new PlayerViewState
            {
                Hand = player.Hand,
                RoundIndex = roundIndex
            };

            var throwCard = client.ChooseThrowCard(view);
            var catchCard = client.ChooseCatchCard(view);

            player.PlayedCards.Add(throwCard);
            player.PlayedCards.Add(catchCard);

            var roundState = new RoundState(player, player.PlayedCards, player.PlayedCards, roundIndex);
            var score = _scoringRules.CalculateRoundScore(roundState);
            player.ScoreSheet.Apply(score);

            client.ShowRoundResult(score);
        }
    }
}
