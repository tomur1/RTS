using System;
using System.Collections.Generic;
using System.Linq;
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
        public Dictionary<int,Group> Groups { get; set; }
        public GameObject MapObject { get; set; }
        public abstract void InitValues(Player player, Vector2Int coord);
        public void Destroyed()
        {
            foreach (var group in Groups.Values.ToList())
            {
                group.RemoveUnit(this);
            }
            
            GameMaster.Instance.grid.RemoveElement(this);
            GameMaster.Instance.StopCoroutinesForObject(this);
            Player.Units.Remove(this);
            GameMaster.Instance.DestroyMapObject(MapObject);
        }
        
        public void SetPlayer(Player player)
        {
            if (player == null)
            {
                if (this.player != null)
                {
                    Player tmp = this.player;
                    this.player = null;
                    tmp.RemoveUnit(this);
                }
            }
            else
            {
                if (this.player != player)
                {
                    if (this.player != null)
                    {
                        this.player.RemoveUnit(this);
                    }

                    player.AddUnit(this);
                    this.player = player;
                }
            }
        }

        public abstract int ConstructionMultiplier { get; set; }
        public Health Health { get; set; }

        public abstract void HandleRightClick(Cell clickedCell);
        
        private Player player;

        public Player Player
        {
            get => player;
            set => SetPlayer(value);
        }

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
            return new List<int>(Groups.Keys);
        }

        public void AddToGroup(int groupNumber)
        {
            Group group;
            if (Groups.ContainsKey(groupNumber))
            {
                group = Groups[groupNumber];
            }
            else
            {
                group = Group.GetGroupWithNumber(groupNumber);
            }
            
            group.AddUnit(this);
        }

        public void RemoveFromGroup(int groupNumber)
        {
            Group group;
            if (Groups.ContainsKey(groupNumber))
            {
                group = Groups[groupNumber];
            }
            else
            {
                group = Group.GetGroupWithNumber(groupNumber);
            }
            
            group.RemoveUnit(this);
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