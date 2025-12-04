using System.Collections.Generic;
using Boomerang.Domain;

namespace Boomerang.Game
{
    public interface IGameResultsPresenter
    {
        void ShowFinalResults(List<Player> players, List<Player> winners);
    }
}
