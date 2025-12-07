using System.Collections.Generic;
using Boomerang.Domain;

namespace Boomerang.Game;

/// <summary>
/// Default implementation of deck creation and shuffling.
/// </summary>

public interface IDeckService
{
    List<Card> CreateShuffledDeck();
}
