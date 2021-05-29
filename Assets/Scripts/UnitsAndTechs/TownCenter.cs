using System;
using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace;
using UnityEngine;

namespace UnitsAndTechs
{
    public class TownCenter : Building, IMenuContainer
    {
        public TownCenter(Vector2Int leftTopCoord, Player player, bool startGame)
        {
            GridSize = new Vector2(3, 3);
            Health = new Health(1000);
            // On game start insert the town center allready constructed
            if (startGame)
            {
                AssetName = "Buildings/Town Center";
                constructionCost = new ConstructionCost(500, 200,
                    0, 0, 0, 500, 500);
            }
            else
            {
                AssetName = "Buildings Transparent/Town Center";
                constructionCost = new ConstructionCost(500, 200,
                    0, 0, 0, 500);
                
            }
            
            LeftTopCellCoord = leftTopCoord;
            Player = player;
            
            var mapElement = GameMaster.Instance.AddElementToGrid(this);
            var component = mapElement.GetComponent<TownCenterUnity>();
            component.TownCenter = this;
        }

        public override int ConstructionMultiplier { get; set; }
        public override Vector2 GridSize { get; set; }
        public override ConstructionCost constructionCost { get; set; }
        public override Player Player { get; set; }
        public override Health Health { get; set; }

        private Dictionary<int, ButtonSpec> ButtonValuesSet;
        public void PerformAction(string action)
        {
            Type thisType = GetType();
            MethodInfo theMethod = thisType.GetMethod(action);
            theMethod.Invoke(this, null);
        }

        public Dictionary<int, ButtonSpec> GetButtonLayout()
        {
            if (ButtonValuesSet != null)
            {
                return ButtonValuesSet;
            }

            Dictionary<int, ButtonSpec> buttons = new Dictionary<int, ButtonSpec>();

            ButtonSpec worker = new ButtonSpec("Worker", 0, "CreateWorker");
            buttons.Add(worker.ButtonIdx, worker);

            return buttons;
        }

        public void CreateWorker()
        {
            //Check if enough resources and the start creation process
            Debug.Log("Creating worker");
        }
    }
}