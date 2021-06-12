namespace UnitsAndTechs
{
    public class ConstructionCost
    {
        // How much material is needed to construct/research
        private int metal;
        private int oil;
        private int uranium;
        private int energy;
        private int science;
        // How hard is it to construct/research this?
        // For example you need 1000 construction points
        private int constructionDifficulty;
        // How much construction is complete
        public float ConstructionPoints { get; set; }

        // Is the construction finished?
        public bool InConstruction => ConstructionPoints < constructionDifficulty;

        public int Metal => metal;

        public int Oil => oil;

        public int Uranium => uranium;

        public int Energy => energy;

        public int Science => science;

        public int ConstructionDifficulty => constructionDifficulty;

        public ConstructionCost(int metal, int oil, int uranium, int energy, int science, int constructionDifficulty)
        {
            this.metal = metal;
            this.oil = oil;
            this.uranium = uranium;
            this.energy = energy;
            this.science = science;
            this.constructionDifficulty = constructionDifficulty;
            ConstructionPoints = 0;
        }
        
        public ConstructionCost(int metal, int oil, int uranium, int energy, int science, int constructionDifficulty, int constructionPoints)
        {
            this.metal = metal;
            this.oil = oil;
            this.uranium = uranium;
            this.energy = energy;
            this.science = science;
            this.constructionDifficulty = constructionDifficulty;
            ConstructionPoints = constructionPoints;
        }
    }
}