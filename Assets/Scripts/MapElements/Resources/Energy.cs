using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : Resource
{
    public Energy()
    {
        MiningDifficulty = 1;
        Capacity = 10000;
        CollectedAmount = 0;
    }
    
    public Energy(int startingAmount)
    {
        MiningDifficulty = 1;
        Capacity = 10000;
        CollectedAmount = startingAmount;
    }
}
