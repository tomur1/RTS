using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Resource
{
    Metal()
    {
        MiningDifficulty = 10;
        Capacity = 1000;
        CollectedAmount = 0;
    }
}
