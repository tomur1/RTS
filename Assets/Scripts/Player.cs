using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
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
    
    Player(Color color)
    {
        buildings = new List<Building>();
        units = new List<Unit>();
        playerScore = 0;
        this.color = color;
    }
    
    
}
