namespace Boomerang.Domain;


/// <summary>
/// Score result for a single round.
/// </summary>

public class RoundScore
{
    public int ThrowCatch { get; set; }
    public int Regions { get; set; }
    public int Collections { get; set; }
    public int Animals { get; set; }
    public int Activities { get; set; }

    public int Total => ThrowCatch + Regions + Collections + Animals + Activities;
}
