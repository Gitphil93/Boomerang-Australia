using System.Collections.Generic;

namespace Boomerang.Domain;

public class Player
{
    public string Name { get; }

    public List<Card> Hand { get; } = new();
    public List<Card> Draft { get; } = new();
    public List<Card> PlayedCards { get; } = new();
    public ScoreSheet ScoreSheet { get; } = new();

    public Player(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}
