namespace UnitsAndTechs
{
    public class ConstructionCost
    {
        // How much material is needed to construct/research
        private int Metal;
        private int Oil;
        private int Uranium;
        private int Energy;
        private int Science;
        // How hard is it to construct/research this?
        // For example you need 1000 construction points
        private int ConstructionDifficulty;
        // How much construction is complete
        public int ConstructionPoints { get; set; }

        // Is the construction finished?
        public bool InConstruction => ConstructionPoints < ConstructionDifficulty;

        public int Metal1 => Metal;

        public int Oil1 => Oil;

        public int Uranium1 => Uranium;

        public int Energy1 => Energy;

        public int Science1 => Science;

        public int ConstructionDifficulty1 => ConstructionDifficulty;

        public ConstructionCost(int metal, int oil, int uranium, int energy, int science, int constructionDifficulty)
        {
            Metal = metal;
            Oil = oil;
            Uranium = uranium;
            Energy = energy;
            Science = science;
            ConstructionDifficulty = constructionDifficulty;
            ConstructionPoints = 0;
        }
        
        public ConstructionCost(int metal, int oil, int uranium, int energy, int science, int constructionDifficulty, int constructionPoints)
        {
            Metal = metal;
            Oil = oil;
            Uranium = uranium;
            Energy = energy;
            Science = science;
            ConstructionDifficulty = constructionDifficulty;
            ConstructionPoints = constructionPoints;
        }
    }
}