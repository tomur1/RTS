using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace;
using UnitsAndTechs.Units;
using UnityEngine;

namespace UnitsAndTechs
{
    public class TownCenter : Building, IMenuContainer, IUnitSpawner
    {
        public TownCenter(bool startGame, Worker worker = null)
        {
            GridSize = new Vector2(3, 3);
            GridMultiplier = 3;
            // On game start insert the town center already constructed
            if (startGame)
            {
                ConstructionCost = new ConstructionCost(500, 200,
                    0, 0, 0, 500, 500);
            }
            else
            {
                AssetName = "Buildings Transparent/Town Center";
                ConstructionCost = new ConstructionCost(500, 200,
                    0, 0, 0, 50);
            }
            Health = Health.CreateAndAssign(0, ConstructionCost.ConstructionDifficulty, this);

        }

        public override int ConstructionMultiplier { get; set; }
        public override Vector2 GridSize { get; set; }
        public override int GridMultiplier { get; set; }

        public override void InitValues(Player player, Vector2Int coord)
        {
            LeftTopCellCoord = coord;
            Player = player;
            Health = Health.CreateAndAssign(500, ConstructionCost.ConstructionDifficulty, this);
            SpawnPoint = LeftTopCellCoord - Vector2Int.one;
            AssetName = "Buildings/Town Center";
            if (!GameMaster.Instance.grid.InBounds(SpawnPoint))
            {
                SpawnPoint = LeftTopCellCoord + Vector2Int.one;
            }

            CreateUnityObject();
        }

        public override void InitValuesFoundation(Player player, Vector2Int coord)
        {
            LeftTopCellCoord = coord;
            Player = player;
            AssetName = "Buildings Transparent/Town Center";
            Health = Health.CreateAndAssign(0, ConstructionCost.ConstructionDifficulty, this);
            GameMaster.Instance.grid.AddElement(this);
        }

        protected override void ConstructionFinished()
        {
            GameMaster.Instance.DestroyMapObject(MapObject);
            GameMaster.Instance.grid.RemoveElement(this);
            InitValues(Player, LeftTopCellCoord);
        }

        public void CreateUnityObject()
        {
            var mapElement = GameMaster.Instance.AddElementToGrid(this);
            var component = mapElement.GetComponent<TownCenterUnity>();
            component.TownCenter = this;
            MapObject = mapElement;
        }
        
        public void CreateFoundationUnityObject(Worker worker)
        {
            var mapElement = GameMaster.Instance.CreateObject(this);
            var component = mapElement.GetComponent<FoundationUnity>();
            var townCenterUnitycomponent = mapElement.GetComponent<TownCenterUnity>();
            component.Element = this;
            component.Worker = worker;
            townCenterUnitycomponent.TownCenter = this;
            MapObject = mapElement;
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
            ButtonSpec soldier = new ButtonSpec("Soldier", 1, "CreateSoldier");
            buttons.Add(worker.ButtonIdx, worker);
            buttons.Add(soldier.ButtonIdx, soldier);
            _buttonValuesSet = buttons;
            return buttons;
        }

        public void CreateWorker()
        {
            //Check if enough resources and the start creation process
            if (Player.CanAddUnitToLimit)
            {
                var worker = new Worker(Player);
                if (Player.HasEnoughResources(worker.ConstructionCost))
                {
                    Player.SubtractResources(worker.ConstructionCost);
                    GameMaster.Instance.StartCoroutine(StartCreatingUnit(worker));
                }
                else
                {
                    GameMaster.Instance.GuiManager.ShowMessage("Not enough resources");
                }
            }
            else
            {
                GameMaster.Instance.GuiManager.ShowMessage("Not enough population");
            }
            
            
        }
        
        public void CreateSoldier()
        {
            //Check if enough resources and the start creation process
            if (Player.CanAddUnitToLimit)
            {
                var soldier = new Soldier(Player);
                if (Player.HasEnoughResources(soldier.ConstructionCost))
                {
                    Player.SubtractResources(soldier.ConstructionCost);
                    GameMaster.Instance.StartCoroutine(StartCreatingUnit(soldier));
                }
                else
                {
                    GameMaster.Instance.GuiManager.ShowMessage("Not enough resources");
                }
            }
            else
            {
                GameMaster.Instance.GuiManager.ShowMessage("Not enough population");
            }
            
        }

        private IEnumerator StartCreatingUnit(IPlaceable unit)
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