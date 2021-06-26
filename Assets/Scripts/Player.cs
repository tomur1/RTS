using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DefaultNamespace;
using TMPro.SpriteAssetUtilities;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

public class Player
{
    public List<Building> Buildings
    {
        get => buildings;
    }

    public List<Unit> Units
    {
        get => units;
    }

    public int PlayerScore
    {
        get => playerScore;
        set => playerScore = value;
    }

    private List<Building> buildings;
    private List<Unit> units;
    private readonly Color color;
    private int playerScore;
    private float researchSpeed = 10;
    private float constructionSpeed = 10;
    private static int MinCapacity = 10;
    private static int MaxCapacity = 200;
    private int UnitCapacityLimit = MinCapacity;
    private int CurrentPopulation = 0;
    private Uranium uranium;
    private Oil oil;
    private Energy energy;
    private Metal metal;
    private Science science;

    public void AddCapacity(int amount)
    {
        UnitCapacityLimit += amount;
        if (UnitCapacityLimit > MaxCapacity)
        {
            UnitCapacityLimit = MaxCapacity;
        }
    }

    public void RemoveCapacity(int amount)
    {
        UnitCapacityLimit -= amount;
        if (UnitCapacityLimit < MinCapacity)
        {
            UnitCapacityLimit = MinCapacity;
        }
    }

    public void AddPopulation(int amount)
    {
        CurrentPopulation += amount;
        if (CurrentPopulation > UnitCapacityLimit)
        {
            throw new Exception("Cannot add units over limit");
        }
    }
    
    public void RemovePopulation(int amount)
    {
        CurrentPopulation -= amount;
        if (CurrentPopulation < 0)
        {
            throw new Exception("Cannot have negative units");
        }
    }

    public bool CanAddUnitToLimit => CurrentPopulation < UnitCapacityLimit;

    public Uranium Uranium
    {
        get => uranium;
        set => uranium = value;
    }

    public Oil Oil
    {
        get => oil;
        set => oil = value;
    }

    public Energy Energy
    {
        get => energy;
        set => energy = value;
    }

    public Metal Metal
    {
        get => metal;
        set => metal = value;
    }

    public Science Science
    {
        get => science;
        set => science = value;
    }

    public float ResearchSpeed
    {
        get => researchSpeed;
        set => researchSpeed = value;
    }

    public float ConstructionSpeed
    {
        get => constructionSpeed;
        set => constructionSpeed = value;
    }

    public float UnitTrainingSpeed
    {
        get => unitTrainingSpeed;
        set => unitTrainingSpeed = value;
    }

    private float unitTrainingSpeed = 10;

    public Player(Color color)
    {
        buildings = new List<Building>();
        units = new List<Unit>();
        playerScore = 0;
        Energy = new Energy(1000);
        Uranium = new Uranium();
        Oil = new Oil(1000);
        Metal = new Metal(1000);
        Science = new Science();
        this.color = color;
    }

    public void AddBuilding(Building building)
    {
        if (!Buildings.Contains(building))
        {
            Buildings.Add(building);
            building.SetPlayer(this);
        }
    }

    public void RemoveBuilding(Building building)
    {
        if (Buildings.Contains(building))
        {
            Buildings.Remove(building);
            building.SetPlayer(null);
        }
    }

    public ReadOnlyCollection<Building> GetBuildings()
    {
        return new ReadOnlyCollection<Building>(Buildings);
    }
    
    public void AddUnit(Unit unit)
    {
        if (!Units.Contains(unit))
        {
            AddPopulation(1);
            Units.Add(unit);
            unit.SetPlayer(this);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        if (Units.Contains(unit))
        {
            RemovePopulation(1);
            Units.Remove(unit);
            unit.SetPlayer(null);
        }
    }

    public ReadOnlyCollection<Unit> GetUnits()
    {
        return new ReadOnlyCollection<Unit>(Units);
    }

    public bool HasEnoughResources(ConstructionCost constructionCost)
    {
        if (constructionCost.Energy > Energy.CollectedAmount)
        {
            Debug.Log("Not enough energy");
            return false;
        }
        else if (constructionCost.Metal > Metal.CollectedAmount)
        {
            Debug.Log("Not enough metal");
            return false;
        }
        else if (constructionCost.Oil > Oil.CollectedAmount)
        {
            Debug.Log("Not enough oil");
            return false;
        }
        else if (constructionCost.Science > Science.CollectedAmount)
        {
            Debug.Log("Not enough science");
            return false;
        }
        else if (constructionCost.Uranium > uranium.CollectedAmount)
        {
            Debug.Log("Not enough uranium");
            return false;
        }

        return true;
    }

    public void SubtractResources(ConstructionCost constructionCost)
    {
        Energy.SubtractResource(constructionCost.Energy);
        Metal.SubtractResource(constructionCost.Metal);
        Uranium.SubtractResource(constructionCost.Uranium);
        Science.SubtractResource(constructionCost.Science);
        Oil.SubtractResource(constructionCost.Oil);
    }
}