using System.Collections.Generic;
using Boomerang.Domain;

namespace Boomerang.Game;

public interface ICardRepository
{
    List<Card> LoadAll();
}
