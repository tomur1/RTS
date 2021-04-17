public class ConstructionCost
{
    // How much material is needed to construct/research
    private int Metal;
    private int Oil;
    private int Uranium;
    private int Energy;
    private int Science;
    // How hard is it to construct/research this?
    private int ConstructionDifficulty;

    public ConstructionCost(int metal, int oil, int uranium, int energy, int science, int constructionDifficulty)
    {
        Metal = metal;
        Oil = oil;
        Uranium = uranium;
        Energy = energy;
        Science = science;
        ConstructionDifficulty = constructionDifficulty;
    }
}