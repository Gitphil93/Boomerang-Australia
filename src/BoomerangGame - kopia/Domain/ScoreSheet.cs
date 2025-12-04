namespace Boomerang.Domain;

public class ScoreSheet
{
    public int ThrowCatchScore { get; set; }
    public int RegionScore { get; set; }
    public int CollectionScore { get; set; }
    public int AnimalScore { get; set; }
    public int ActivityScore { get; set; }

    public int TotalScore =>
        ThrowCatchScore + RegionScore + CollectionScore + AnimalScore + ActivityScore;

    public void Apply(RoundScore roundScore)
    {
        ThrowCatchScore += roundScore.ThrowCatch;
        RegionScore += roundScore.Regions;
        CollectionScore += roundScore.Collections;
        AnimalScore += roundScore.Animals;
        ActivityScore += roundScore.Activities;
    }
}
