using Boomerang.Domain;

namespace Boomerang.Game;

public class AustraliaScoringRules : IScoringRules
{
    public RoundScore CalculateRoundScore(RoundState state)
    {
        // TODO: implementera riktiga regler (Throw & Catch, regions, collections, animals, activities)
        return new RoundScore();
    }
}
