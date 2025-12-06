using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boomerang.Game;

public class GameConfig
{
    public const int DefaultRounds = 4;
    public const int DefaultCardsPerPlayer = 7;

    public int Rounds { get; init; } = DefaultRounds;
    public int CardsPerPlayer { get; init; } = DefaultCardsPerPlayer;

    public GameConfig() { }

    public GameConfig(int rounds, int cardsPerPlayer)
    {
        Rounds = rounds;
        CardsPerPlayer = cardsPerPlayer;
    }
}
