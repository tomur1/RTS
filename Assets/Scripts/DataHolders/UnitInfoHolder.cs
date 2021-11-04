using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

namespace DataHolders
{
    public class UnitInfoHolder
    {
        private readonly Unit unit;
        public Health Health { get; set; }
        public Vector2Int LeftTopCellCoord { get; set; }
        public string TypeName { get; set; }

        public ConstructionCost Cost { get; set; }

        public List<int> Groups { get; set; }

        public UnitInfoHolder()
        {
        
        }
    
        public UnitInfoHolder(Unit unit)
        {
            this.unit = unit;
            Health = unit.Health;
            LeftTopCellCoord = unit.LeftTopCellCoord;
            TypeName = unit.GetType().Name;
            Groups = unit.GroupsNumbers();
            Cost = unit.ConstructionCost;
        }
    }
}