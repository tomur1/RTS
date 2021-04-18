using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : Resource
{
    Energy()
    {
        MiningDifficulty = 1;
        Capacity = 10000;
        CollectedAmount = 0;
    }
}
