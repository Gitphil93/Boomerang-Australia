using System.Collections.Generic;
using Boomerang.Domain;

/// <summary>
/// Holds final results and winners.
/// </summary>


namespace Boomerang.Game
{
    public class GameResult
    {
        public List<Player> Players { get; }
        public List<Player> Winners { get; }
        public bool IsTie => Winners.Count > 1;

        public GameResult(List<Player> players, List<Player> winners)
        {
            Players = players;
            Winners = winners;
        }
    }
}
