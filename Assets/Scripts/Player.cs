using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro.SpriteAssetUtilities;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

public class Player
{
    private List<Building> buildings;

    public List<Building> Buildings
    {
        get => buildings;
        set => buildings = value;
    }

    public List<Unit> Units
    {
        get => units;
        set => units = value;
    }

    public int PlayerScore
    {
        get => playerScore;
        set => playerScore = value;
    }

    private List<Unit> units;
    private readonly Color color;
    private int playerScore;
    private float researchSpeed = 10;
    private float constructionSpeed = 10;
    private Uranium uranium;
    private Oil oil;
    private Energy energy;
    private Metal metal;
    private Science science;

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
        Energy = new Energy(100);
        Uranium = new Uranium();
        Oil = new Oil();
        Metal = new Metal(100);
        Science = new Science();
        this.color = color;
    }
    
    public bool HasEnoughResources(ConstructionCost constructionCost)
    {
        if (constructionCost.Energy > Energy.CollectedAmount)
        {
            Debug.Log("Not enough energy");
            return false;
        }else if (constructionCost.Metal > Metal.CollectedAmount)
        {
            Debug.Log("Not enough metal");
            return false;
        }else if (constructionCost.Oil > Oil.CollectedAmount)
        {
            Debug.Log("Not enough oil");
            return false;
        }else if (constructionCost.Science > Science.CollectedAmount)
        {
            Debug.Log("Not enough science");
            return false;
        }else if (constructionCost.Uranium > uranium.CollectedAmount)
        {
            Debug.Log("Not enough uranium");
            return false;
        }

        return true;
    }

    public void SubstractResources(ConstructionCost constructionCost)
    {
        Energy.CollectedAmount -= constructionCost.Energy;
        Metal.CollectedAmount -= constructionCost.Metal;
        Uranium.CollectedAmount -= constructionCost.Uranium;
        Science.CollectedAmount -= constructionCost.Science;
        Oil.CollectedAmount -= constructionCost.Oil;
    }
    
    
}
