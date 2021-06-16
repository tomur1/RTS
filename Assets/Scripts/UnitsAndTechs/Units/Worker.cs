using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

public class Worker : Unit, IMenuContainer
{
    public Worker()
    {
        ConstructionCost = new ConstructionCost(50, 0, 0, 0, 0, 50);
    }
    
    //Used for creation on map generation
    public Worker(Player player, Vector2Int coord) : this()
    {
        InitValues(player, coord);
    }

    public override void InitValues(Player player, Vector2Int coord)
    {
        GridSize = Vector2.one;
        Health = new Health(10);
        Player = player;
        LeftTopCellCoord = coord;
        AssetName = "Units/Worker";
        
        var mapElement = GameMaster.Instance.AddElementToGrid(this);
        var component = mapElement.GetComponent<WorkerUnity>();
        component.Worker = this;
    }

    public override Vector2 GridSize { get; set; }

    public override int ConstructionMultiplier { get; set; }
    public override Player Player { get; set; }
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

    public void BuildTownCenter()
    {
        
    }
}
