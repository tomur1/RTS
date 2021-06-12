using System.Collections;
using System.Collections.Generic;
using UnitsAndTechs;
using UnityEngine;

public abstract class Technology : MonoBehaviour
{
    protected Technology()
    {
        PercentResearched = 0;
    }

    public ConstructionCost Cost { get; }
    public float PercentResearched { get; set; }

    public void StartResearch(Player player)
    {
        StartCoroutine(PerformResearch(player));
    }

    private IEnumerator PerformResearch(Player player)
    {
        while (PercentResearched < 100)
        {
            yield return null;
            PercentResearched += (player.ResearchSpeed / Cost.ConstructionDifficulty) * Time.deltaTime;
            //UpdateGui()
        }
        ResearchFinished();
    }

    public abstract void ResearchFinished();

}
