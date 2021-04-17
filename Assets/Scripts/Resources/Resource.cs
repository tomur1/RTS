using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource
{
    protected int miningDifficulty;

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

    protected int collectedAmount;
    protected int capacity;
}
