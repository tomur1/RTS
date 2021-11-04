using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

public class Soldier : Unit
{
    public Soldier(Player player)
    {
        ConstructionCost = new ConstructionCost(150, 0, 0, 0, 0, 20);
        Player = player;
    }

    public override Vector2 GridSize { get; set; }
    public override void InitValues(Player player, Vector2Int coord)
    {
        GridSize = Vector2.one;
        Health.CreateAndAssign(150, 150, this);
        Player = player;
        LeftTopCellCoord = coord;
        AssetName = "Units/Soldier";
        Groups = new Dictionary<int, Group>();
        UnitState = UnitState.Standing;
        AttackAbility.CreateAndAssign(4, 5, 1f, this);
        var mapElement = GameMaster.Instance.AddElementToGrid(this);
        var component = mapElement.GetComponent<SoldierUnity>();
        component.Soldier = this;
        MapObject = mapElement;
    }

    public override int ConstructionMultiplier { get; set; }
    public override void HandleRightClick(Cell clickedCell)
    {
        bool attack = false;
        foreach (var placeable in clickedCell.Elements)
        {
            if (placeable.Player != Player)
            {
                GameMaster.Instance.Attack(this, placeable);
                attack = true;
                break;
            }

        }
        
        if (clickedCell.canPass() && !attack)
        {
            MoveTo(clickedCell);
        }
        
        
    }

    public void Attack(Object[] targetParam)
    {
        IPlaceable target = (IPlaceable) targetParam[0];
        GameMaster.Instance.Attack(this, target);
    }
    
    public override int getRange()
    {
        return AttackAbility.range;
    }

    public AttackAbility AttackAbility { get; set; }
}
