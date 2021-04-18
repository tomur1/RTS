using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    public abstract Vector2 GridSize { get; set; }
    public GridMode GridMode { get; }
    public int GridMultiplier { get; }
    public string AssetName { get; set; }
    public Vector2Int LeftTopCellCoord { get; set; }
    
}