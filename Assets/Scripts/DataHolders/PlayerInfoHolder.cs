using System;
using System.Collections;
using System.Collections.Generic;
using DataHolders;
using UnityEngine;

public class PlayerInfoHolder
{
    public List<BuildingInfoHolder> Buildings { get; set; }
    
    public List<UnitInfoHolder> Units { get; set; }
    public int PlayerScore { get; set; }
    public Color Color { get; set; }
    public Uranium Uranium { get; set; }
    public Oil Oil { get; set; }
    public Energy Energy { get; set; }
    public Metal Metal { get; set; }
    public Science Science { get; set; }

    public PlayerInfoHolder()
    {
        
    }

    public PlayerInfoHolder(Player player)
    {
        Buildings = new List<BuildingInfoHolder>();
        Units = new List<UnitInfoHolder>();
        foreach (var building in player.Buildings)
        { 
            Buildings.Add(new BuildingInfoHolder(building));
        }
        foreach (var unit in player.Units)
        {
            Units.Add(new UnitInfoHolder(unit));
        }

        PlayerScore = player.PlayerScore;

        Color = player.Color;

        Uranium = player.Uranium;
        Oil = player.Oil;
        Energy = player.Energy;
        Metal = player.Metal;
        Science = player.Science;
    }
}
