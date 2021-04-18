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
    }
}