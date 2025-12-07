using System.Collections.Generic;
using Boomerang.Domain;

/// <summary>
/// Shows high-level game messages.
/// </summary>


namespace Boomerang.Game
{
    public interface IGamePresenter
    {
        void ShowGameStart(int playerCount);
        void ShowRoundStart(int roundIndex);
        void ShowFinalResults(List<Player> players, List<Player> winners);
      
    }
}
