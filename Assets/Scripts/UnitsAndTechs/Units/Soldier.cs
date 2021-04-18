using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs.Units;
using UnityEngine;

public class Soldier : Unit
{
    public override Vector2 GridSize { get; set; }
    public override int ConstructionMultiplier { get; set; }
    public static AttackAbility attackAbility;

    public Soldier()
    {
    }
}
