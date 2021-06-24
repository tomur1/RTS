using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Resource
{
    public Metal()
    {
        MiningDifficulty = 10;
        Capacity = 1000;
        CollectedAmount = 0;
    }
    
    public Metal(int startingAmount)
    {
        MiningDifficulty = 10;
        Capacity = 1000;
        CollectedAmount = startingAmount;
    }
}
