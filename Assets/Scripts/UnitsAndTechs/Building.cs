public abstract class Building : Placeable
{
    private static int ConstructionMultiplier;
    public const int GRIDMULTIPLIER = 3;
    public override GridMode GridMode => GridMode.blocking;
    public abstract ConstructionCost constructionCost { get; set; }
}