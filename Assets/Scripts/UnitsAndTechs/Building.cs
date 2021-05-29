using DefaultNamespace;
using UnitsAndTechs;
using UnityEngine;

public abstract class Building : IPlaceable  
{
    public abstract int ConstructionMultiplier { get; set; }

    public abstract Vector2 GridSize { get; set; }
    public GridMode GridMode { get => GridMode.blocking; }
    public int GridMultiplier { get => 3; }
    
    public string AssetName { get; set; }
    public Vector2Int LeftTopCellCoord { get; set; }

    public abstract ConstructionCost constructionCost { get; set; }

    public abstract Player Player { get; set; }

    public abstract Health Health { get; set; }
}