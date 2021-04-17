using UnityEngine;

public abstract class Building : MonoBehaviour, IPlaceable  
{
    public abstract int ConstructionMultiplier { get; set; }

    public abstract Vector2 GridSize { get; set; }
    public GridMode GridMode { get; set; }
    public int GridMultiplier { get; set; }
    
    public string AssetName { get; set; }
    public Vector2Int LeftTopCellCoord { get; set; }

    public abstract ConstructionCost constructionCost { get; set; }
}