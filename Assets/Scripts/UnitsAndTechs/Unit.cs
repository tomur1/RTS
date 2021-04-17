using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Unit : MonoBehaviour, IPlaceable
    {
        public Vector2 GridSize { get; set; }
        GridMode IPlaceable.GridMode { get; set; }
        public int GridMultiplier { get; set; }
        public string AssetName { get; set; }
        public Vector2Int LeftTopCellCoord { get; set; }
        public abstract int ConstructionMultiplier { get; set; }
    }
}