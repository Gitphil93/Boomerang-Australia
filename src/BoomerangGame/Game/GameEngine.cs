using System;
using System.Collections.Generic;
using System.Linq;
using Boomerang.Domain;
using Boomerang.IO;
using Boomerang.Game;

/// <summary>
/// Orchestrates game flow and uses dependencies to run a complete game
/// </summary>


namespace Boomerang.Game;

public class GameEngine
{
    private readonly List<Player> _players;
    private readonly IScoringRules _scoringRules;
    private readonly IPassDirectionStrategy _passDirection;
    private readonly IDeckService _deckService;
    private readonly IPlayerClient[] _clients;
    private readonly IGamePresenter _presenter;
    private readonly GameConfig _config;
    public GameEngine(
        List<Player> players,
        IPlayerClient[] clients,
        IDeckService deckService,
        IScoringRules scoringRules,
        IPassDirectionStrategy passDirection,
        IGamePresenter presenter,
        GameConfig config)
    {
        _players = players;
        _clients = clients;
        _deckService = deckService;
        _scoringRules = scoringRules;
        _passDirection = passDirection;
        _presenter = presenter;
        _config = config;
    }

 
    public void RunGame()
    {
        _presenter.ShowGameStart(_players.Count);

        for (int round = 0; round < _config.Rounds; round++) 
        {
            RunRound(round);
        }
        var final = CalculateFinalResults();
        _presenter.ShowFinalResults(final.Players, final.Winners);
    }


    public void RunRound(int roundIndex)
    {
        _presenter.ShowRoundStart(roundIndex + 1);

        var deck = _deckService.CreateShuffledDeck();

        ResetRoundState();
        DealHands(deck);
        ChooseThrowCards(roundIndex);

        var hands = CreatePassingHandsSnapshot();
      
        RunDraftLoop(roundIndex, hands);
        ApplyCatchCards(roundIndex, hands);

        ScoreRound(roundIndex);
    }
    private void ResetRoundState()
    {
        foreach (var player in _players)
        {
            player.Hand.Clear();
            player.PlayedCards.Clear();
            player.Draft.Clear();
        }
    }

    private void DealHands(List<Card> deck)
    {
        
        int deckIndex = 0;

        foreach (var player in _players)
        {
            for (int j = 0; j < _config.CardsPerPlayer; j++)
            {
                if (deckIndex >= deck.Count)
                    throw new InvalidOperationException("Deck ran out of cards.");

                player.Hand.Add(deck[deckIndex++]);
            }
        }
    }
    private void ChooseThrowCards(int roundIndex)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            var player = _players[i];
            var client = _clients[i];

            var view = new PlayerViewState
            {
                Hand = player.Hand.ToList(), 
                RoundIndex = roundIndex
            };

            var throwCard = client.ChooseThrowCard(view);

            if (!player.Hand.Remove(throwCard))
                throw new InvalidOperationException("Chosen throw card was not in player's hand.");

            player.PlayedCards.Add(throwCard);
        }
    }
    private List<List<Card>> CreatePassingHandsSnapshot()
    {
        
        return _players
            .Select(p => new List<Card>(p.Hand))
            .ToList();
    }

    private void RunDraftLoop(int roundIndex, List<List<Card>> hands)
    {
        
        while (true)
        {
            
            var draftedThisStep = new Card[_players.Count];

            for (int i = 0; i < _players.Count; i++)
            {
                var hand = hands[i];
                if (hand.Count == 0)
                    throw new InvalidOperationException("Hand empty during drafting.");


                var chosen = hand[0];
                hand.RemoveAt(0);
                draftedThisStep[i] = chosen;
            }

           
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].PlayedCards.Add(draftedThisStep[i]);
            }

         
            if (hands.All(h => h.Count == 1))
                break;

           
            hands = PassHands(roundIndex, hands);
        }
    }

    private List<List<Card>> PassHands(int roundIndex, List<List<Card>> hands)
    {
        var newHands = new List<List<Card>>(_players.Count);
        for (int i = 0; i < _players.Count; i++)
        {
            newHands.Add(new List<Card>());
        }

        for (int i = 0; i < _players.Count; i++)
        {
            int next = _passDirection.GetNextPlayerIndex(i, roundIndex, _players.Count);
            newHands[next] = hands[i];
        }

        return newHands;
    }

    private void ApplyCatchCards(int roundIndex, List<List<Card>> hands)
    {
        var catchCards = new Card[_players.Count];

        for (int i = 0; i < _players.Count; i++)
        {
            if (hands[i].Count != 1)
                throw new InvalidOperationException("Expected exactly 1 card in hand before catch phase.");

            var lastCard = hands[i][0];

            int receiver = GetPreviousPlayerIndex(i, roundIndex);
            catchCards[receiver] = lastCard;
        }

        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].PlayedCards.Add(catchCards[i]);
        }

     
        foreach (var player in _players)
        {
            if (player.PlayedCards.Count != 7)
                throw new InvalidOperationException($"Player {player.Name} does not have 7 cards.");
        }
    }

    private void ScoreRound(int roundIndex)
    {
        var allCardsThisRound = _players.SelectMany(p => p.PlayedCards).ToList();

        for (int i = 0; i < _players.Count; i++)
        {
            var player = _players[i];

            var roundState = new RoundState(
                player,
                player.PlayedCards.ToList(),
                allCardsThisRound,
                roundIndex
            );

            var score = _scoringRules.CalculateRoundScore(roundState);
            player.ScoreSheet.Apply(score);

            _clients[i].ShowRoundResult(score);
        }
    }

    private GameResult CalculateFinalResults()
    {
        int maxTotal = _players.Max(p => p.ScoreSheet.TotalScore);
        var top = _players.Where(p => p.ScoreSheet.TotalScore == maxTotal).ToList();

        if (top.Count == 1)
            return new GameResult(_players, top);

        int maxThrowCatch = top.Max(p => p.ScoreSheet.ThrowCatchScore);
        var final = top.Where(p => p.ScoreSheet.ThrowCatchScore == maxThrowCatch).ToList();

        return new GameResult(_players, final);
    }


    private int GetPreviousPlayerIndex(int currentIndex, int roundIndex)
    {
       
        for (int i = 0; i < _players.Count; i++)
        {
            int next = _passDirection.GetNextPlayerIndex(i, roundIndex, _players.Count);
            if (next == currentIndex)
                return i;
        }

      
        return (currentIndex - 1 + _players.Count) % _players.Count;
    }
}
