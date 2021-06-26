using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;
using Object = System.Object;

public class Worker : Unit, IMenuContainer
{
    public Worker(Player player)
    {
        ConstructionCost = new ConstructionCost(50, 0, 0, 0, 0, 10);
        Player = player;
    }
    
    //Used for creation on map generation
    public Worker(Player player, Vector2Int coord) : this(player)
    {
        InitValues(player, coord);
    }

    public override void InitValues(Player player, Vector2Int coord)
    {
        GridSize = Vector2.one;
        Health.CreateAndAssign(50, 50, this);
        Player = player;
        LeftTopCellCoord = coord;
        BuildingSpeed = 10;
        AssetName = "Units/Worker";
        Groups = new Dictionary<int, Group>();
        UnitState = UnitState.Standing;
        var mapElement = GameMaster.Instance.AddElementToGrid(this);
        var component = mapElement.GetComponent<WorkerUnity>();
        component.Worker = this;
        MapObject = mapElement;
    }
    
    public override int getRange()
    {
        return 1;
    }

    public int BuildingSpeed { get; set; }

    public override Vector2 GridSize { get; set; }

    public override int ConstructionMultiplier { get; set; }
    public override void HandleRightClick(Cell clickedCell)
    {
        if (clickedCell.canPass())
        {
            MoveTo(clickedCell);
            return;
        }

        foreach (var placeable in clickedCell.Elements)
        {
            if (placeable.GetType() == typeof(TownCenter))
            {
                if (placeable.Player == Player && (placeable.ConstructionCost.InConstruction || placeable.Health.NotFull))
                {
                    GameMaster.Instance.MoveUnitWithAction(this, "RepairOrBuild", placeable);
                }
            }
        }
    }
    
    private Dictionary<int, ButtonSpec> _buttonValuesSet;

    public Dictionary<int, ButtonSpec> GetButtonLayout()
    {
        if (_buttonValuesSet != null)
        {
            return _buttonValuesSet;
        }

        Dictionary<int, ButtonSpec> buttons = new Dictionary<int, ButtonSpec>();

        ButtonSpec worker = new ButtonSpec("Town Center", 0, "BuildTownCenter");
        buttons.Add(worker.ButtonIdx, worker);

        _buttonValuesSet = buttons;
        return buttons;
    }

    public void RepairOrBuild(Object[] buildingParam)
    {
        Building building = (Building) buildingParam[0];
        GameMaster.Instance.RepairBuilding(this, building);
    }

    public void BuildTownCenter()
    {
        var tc = new TownCenter(false);
        if (Player.HasEnoughResources(tc.ConstructionCost))
        {
            Player.SubtractResources(tc.ConstructionCost);
            tc.CreateFoundationUnityObject(this);
            GameMaster.Instance.mode = Mode.Placing;
        }
        else
        {
            GameMaster.Instance.GuiManager.ShowMessage("Not enough resources");
        }
        
        
    }
}
