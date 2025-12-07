using Boomerang.Domain;

namespace Boomerang.Game;

/// <summary>
/// Defines how round scoring is calculated.
/// </summary>

public interface IScoringRules
{
    RoundScore CalculateRoundScore(RoundState state);
}
