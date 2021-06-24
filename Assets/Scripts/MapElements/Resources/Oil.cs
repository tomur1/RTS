using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : Resource
{
    public Oil()
    {
        MiningDifficulty = 20;
        Capacity = 500;
        CollectedAmount = 0;
    }

    public Oil(int startingAmount)
    {
        MiningDifficulty = 20;
        Capacity = 500;
        CollectedAmount = startingAmount;
    }
}
