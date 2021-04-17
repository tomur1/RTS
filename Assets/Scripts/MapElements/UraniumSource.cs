using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UraniumSource : IPlaceable
{
    public Vector2 GridSize { get; set; }
    public GridMode GridMode { get; set; }
    public int GridMultiplier { get; set; }
    
    public string AssetName { get; set; }
    public Vector2Int LeftTopCellCoord { get; set; }

    public UraniumSource(Vector2Int leftTopCellCoord)
    {
        GridMultiplier = 2;
        GridSize = new Vector2(1, 1);
        GridMode = GridMode.blocking;
        AssetName = "Map Resources/Uranium Resource";
        LeftTopCellCoord = leftTopCellCoord;
    }
}
