using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
        this.color = color;
    }
    
    
}
