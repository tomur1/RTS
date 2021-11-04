using UnitsAndTechs;
using UnityEngine;

namespace DataHolders
{
    public class MapElementInfoHolder
    {
        public Vector2Int LeftTopCellCoord { get; set; }
        public string TypeName { get; set; }

        public MapElementInfoHolder()
        {
        
        }
    
        public MapElementInfoHolder(IPlaceable placeable)
        {
            LeftTopCellCoord = placeable.LeftTopCellCoord;
            TypeName = placeable.GetType().Name;
        }
    }
}