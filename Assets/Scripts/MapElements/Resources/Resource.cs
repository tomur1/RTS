using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource
{
    private int miningDifficulty;

    public int MiningDifficulty
    {
        get => miningDifficulty;
        set => miningDifficulty = value;
    }

    public int CollectedAmount
    {
        get => collectedAmount;
        set => collectedAmount = value;
    }

    public int Capacity
    {
        get => capacity;
        set => capacity = value;
    }

    public void AddResource(int amount)
    {
        CollectedAmount += amount;
        if (CollectedAmount < Capacity)
        {
            CollectedAmount = Capacity;
        }
    }

    public void SubtractResource(int amount)
    {
        CollectedAmount -= amount;
        if (CollectedAmount < 0)
        {
            CollectedAmount = 0;
        }
    }

    private int collectedAmount;
    private int capacity;
}
