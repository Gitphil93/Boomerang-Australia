using System.Collections.Generic;
using Boomerang.Domain;

namespace Boomerang.Game;

public interface IDeckService
{
    List<Card> CreateShuffledDeck();
}
