using UnityEngine;

namespace UnitsAndTechs
{
    public interface IPlaceable
    {
        public abstract Vector2 GridSize { get; set; }
        public GridMode GridMode { get; }
        public int GridMultiplier { get; }
        public string AssetName { get; set; }
        public Vector2Int LeftTopCellCoord { get; set; }
        public ConstructionCost ConstructionCost { get; set; }

        public void InitValues(Player player, Vector2Int coord);
    }
}