using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Science : Resource
{
    public Science()
    {
        MiningDifficulty = 30;
        Capacity = 999999999;
        CollectedAmount = 0;
    }
    
    public Science(int startingAmount)
    {
        MiningDifficulty = 30;
        Capacity = 999999999;
        CollectedAmount = startingAmount;
    }
}
