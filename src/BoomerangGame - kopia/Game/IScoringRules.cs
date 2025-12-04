using Boomerang.Domain;

namespace Boomerang.Game;

public interface IScoringRules
{
    RoundScore CalculateRoundScore(RoundState state);
}
