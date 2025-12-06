using System.Collections.Generic;
using Boomerang.Domain;
using Boomerang.IO;

namespace Boomerang.Game;

public interface IGameSetup
{
    (List<Player> players, IPlayerClient[] clients) ConfigurePlayers();
}
