using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uranium : Resource
{
    public Uranium()
    {
        MiningDifficulty = 50;
        Capacity = 100;
        CollectedAmount = 0;
    }
    
    public Uranium(int startingAmount)
    {
        MiningDifficulty = 50;
        Capacity = 100;
        CollectedAmount = startingAmount;
    }
}
