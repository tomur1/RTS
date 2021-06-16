using System.Collections;
using System.Collections.Generic;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

public class Group : MonoBehaviour
{
    private HashSet<Unit> units;

    public HashSet<Unit> Units
    {
        get { return units; }
        set { units = value; }
    }
}
