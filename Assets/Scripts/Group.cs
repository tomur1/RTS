using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

public class Group : MonoBehaviour
{
    private int number;
    private HashSet<Unit> units;

    public int Number
    {
        get => number;
        set => number = value;
    }

    public HashSet<Unit> Units
    {
        get { return units; }
        set { units = value; }
    }
}
