using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnityEngine;

public class UraniumSource : IPlaceable
{
    public Vector2 GridSize { get; set; }
    public GridMode GridMode { get; set; }
    public int GridMultiplier { get; set; }
    
    public string AssetName { get; set; }
    public Vector2Int LeftTopCellCoord { get; set; }
    public ConstructionCost ConstructionCost { get; set; }
    public Player Player { get; set; }
    public Health Health { get; set; }
    public GameObject MapObject { get; set; }

    public void InitValues(Player player, Vector2Int coord)
    {
        
    }

    public UraniumSource(Vector2Int leftTopCellCoord)
    {
        GridMultiplier = 2;
        GridSize = new Vector2(1, 1);
        GridMode = GridMode.blocking;
        AssetName = "Map Resources/Uranium Resource";
        LeftTopCellCoord = leftTopCellCoord;
        
        var mapElement = GameMaster.Instance.AddElementToGrid(this);
        var component = mapElement.GetComponent<UraniumUnity>();
        component.Uranium = this;
        MapObject = mapElement;
    }
}
