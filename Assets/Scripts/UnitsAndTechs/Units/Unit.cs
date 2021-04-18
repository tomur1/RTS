using DefaultNamespace;
using UnityEngine;

namespace UnitsAndTechs.Units
{
    public abstract class Unit : MonoBehaviour, IPlaceable
    {
        public abstract Vector2 GridSize { get; set; }

        GridMode IPlaceable.GridMode => GridMode.passthrough;

        public int GridMultiplier => 1;

        public string AssetName { get; set; }
        public Vector2Int LeftTopCellCoord { get; set; }
        public abstract int ConstructionMultiplier { get; set; }
        public Health Health { get; set; }
    }
}