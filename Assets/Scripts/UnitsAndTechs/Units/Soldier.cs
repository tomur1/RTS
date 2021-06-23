using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs.Units;
using UnityEngine;

public class Soldier : Unit
{
    public override Vector2 GridSize { get; set; }
    public override void InitValues(Player player, Vector2Int coord)
    {
        throw new System.NotImplementedException();
    }

    public override int ConstructionMultiplier { get; set; }
    public override void HandleRightClick(Cell clickedCell)
    {
        throw new System.NotImplementedException();
    }

    public override Player Player { get; set; }
    public static AttackAbility attackAbility;

    public Soldier()
    {
        
    }
}
