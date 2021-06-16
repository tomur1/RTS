using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace UnitsAndTechs.Units
{
    public abstract class Unit : IPlaceable
    {
        public abstract Vector2 GridSize { get; set; }

        GridMode IPlaceable.GridMode => GridMode.passthrough;

        public int GridMultiplier => 1;

        public string AssetName { get; set; }
        public Vector2Int LeftTopCellCoord { get; set; }
        public ConstructionCost ConstructionCost { get; set; }
        public HashSet<Group> Groups { get; set; }
        public abstract void InitValues(Player player, Vector2Int coord);
        public abstract int ConstructionMultiplier { get; set; }
        public Health Health { get; set; }
        
        public abstract Player Player { get; set; }

        public void MoveTo(Cell cell)
        {
            
        }
    }
}