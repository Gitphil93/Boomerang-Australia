using System.Collections.Generic;
using Boomerang.Domain;

namespace Boomerang.IO;

public class PlayerViewState
{
    public List<Card> Hand { get; set; } = new();
    public int RoundIndex { get; set; }
}
