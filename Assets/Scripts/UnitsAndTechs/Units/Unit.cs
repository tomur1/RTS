using System;
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

        public float MovementSpeed = 15f;

        public string AssetName { get; set; }
        public Vector2Int LeftTopCellCoord { get; set; }
        public ConstructionCost ConstructionCost { get; set; }
        public HashSet<Group> Groups { get; set; }
        public GameObject MapObject { get; set; }
        public abstract void InitValues(Player player, Vector2Int coord);
        public abstract int ConstructionMultiplier { get; set; }
        public Health Health { get; set; }

        public abstract void HandleRightClick(Cell clickedCell);
        
        public abstract Player Player { get; set; }

        public UnitState UnitState;

        public void MoveTo(Cell cell)
        {
            var gameMaster = GameMaster.Instance;
            var path = Pathfinding.Instance.FindPath(new Vector3(LeftTopCellCoord.x, 0, LeftTopCellCoord.y), new Vector3(cell.GridPosition.x, 0 , cell.GridPosition.y));
            if (path == null)
            {
                return;
            }

            var rb = MapObject.GetComponent<Rigidbody>();
            
            gameMaster.MoveUnit(this, path);
        }

        public abstract int getRange();
        

        public List<int> GroupsNumbers()
        {
            List<int> groups = new List<int>();
            foreach (var group in Groups)
            {
                groups.Add(group.Number);
            }

            return groups;
        }

        public String GroupsNumberString()
        {
            var res = "Groups: ";
            foreach (var number in GroupsNumbers())
            {
                res += number + ", ";
            }

            return res.Remove(res.Length - 2);
        }
        
    }
}