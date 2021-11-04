using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnityEngine;

public class BuildingInfoHolder
{
    public Health Health { get; set; }
    public Vector2Int LeftTopCellCoord { get; set; }
    public string TypeName { get; set; }
    
    public ConstructionCost Cost { get; set; }
    
    public BuildingInfoHolder()
    {
        
    }
    
    public BuildingInfoHolder(Building building)
    {
        Health = building.Health;
        LeftTopCellCoord = building.LeftTopCellCoord;
        TypeName = building.GetType().Name;
        Cost = building.ConstructionCost;
    }

}
