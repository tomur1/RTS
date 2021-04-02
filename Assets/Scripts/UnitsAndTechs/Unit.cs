namespace DefaultNamespace
{
    public abstract class Unit : Placeable
    {
        public override GridMode GridMode => GridMode.passthrough;
        private static int ConstructionMultiplier;
        private const int GRIDMULTIPLIER = 1;
    }
}