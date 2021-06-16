using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace;
using UnityEngine;

namespace UnitsAndTechs
{
    public class TownCenter : Building, IMenuContainer, IUnitSpawner
    {
        public TownCenter(Vector2Int leftTopCoord, Player player, bool startGame)
        {
            GridSize = new Vector2(3, 3);
            Health = new Health(1000);
            
            // On game start insert the town center already constructed
            if (startGame)
            {
                AssetName = "Buildings/Town Center";
                ConstructionCost = new ConstructionCost(500, 200,
                    0, 0, 0, 500, 500);
            }
            else
            {
                AssetName = "Buildings Transparent/Town Center";
                ConstructionCost = new ConstructionCost(500, 200,
                    0, 0, 0, 500);
                
            }
            
            LeftTopCellCoord = leftTopCoord;
            Player = player;
            SpawnPoint = LeftTopCellCoord - Vector2Int.one;
            
            if (!GameMaster.Instance.grid.InBounds(SpawnPoint))
            {
                SpawnPoint = LeftTopCellCoord + Vector2Int.one;
            }
            
            var mapElement = GameMaster.Instance.AddElementToGrid(this);
            var component = mapElement.GetComponent<TownCenterUnity>();
            component.TownCenter = this;
        }

        public override int ConstructionMultiplier { get; set; }
        public override Vector2 GridSize { get; set; }

        public override void InitValues(Player player, Vector2Int coord)
        {
            throw new NotImplementedException();
        }
        public Vector2Int SpawnPoint { get; set; }

        private Dictionary<int, ButtonSpec> _buttonValuesSet;

        public Dictionary<int, ButtonSpec> GetButtonLayout()
        {
            if (_buttonValuesSet != null)
            {
                return _buttonValuesSet;
            }

            Dictionary<int, ButtonSpec> buttons = new Dictionary<int, ButtonSpec>();

            ButtonSpec worker = new ButtonSpec("Worker", 0, "CreateWorker");
            buttons.Add(worker.ButtonIdx, worker);

            _buttonValuesSet = buttons;
            return buttons;
        }

        public void CreateWorker()
        {
            //Check if enough resources and the start creation process
            GameMaster.Instance.StartCoroutine(StartCreating(new Worker()));
        }

        private IEnumerator StartCreating(IPlaceable unit)
        {
            var constructionCost = unit.ConstructionCost;
            while (constructionCost.InConstruction)
            {
                yield return new WaitForSeconds(1);
                constructionCost.ConstructionPoints += 1 * Player.ConstructionSpeed;
                Debug.Log("Constructing: " + constructionCost.ConstructionPoints + " out of " + constructionCost.ConstructionDifficulty);
            }
            
            unit.InitValues(Player, GameMaster.Instance.grid.FindClosestEmptyPos(unit, SpawnPoint).GridPosition);
        }

        
    }
}