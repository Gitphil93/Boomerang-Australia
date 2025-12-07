using System.Collections.Generic;
using Boomerang.Domain;
using Boomerang.IO;

/// <summary>
/// Creates players and their corresponding clients.
/// </summary>


namespace Boomerang.Game;

public interface IGameSetup
{
    (List<Player> players, IPlayerClient[] clients) ConfigurePlayers();
}
