using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uranium : Resource
{
    Uranium()
    {
        MiningDifficulty = 50;
        Capacity = 100;
        CollectedAmount = 0;
    }
}
