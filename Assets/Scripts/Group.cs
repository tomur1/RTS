using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

public class Group
{
    private int number;
    private List<Unit> units;
    private static List<Group> AllGroups = new List<Group>();

    public static ReadOnlyCollection<Group> GetAllGroups()
    {
        return new ReadOnlyCollection<Group>(AllGroups);
    }

    public static Group GetGroupWithNumber(int groupNumber)
    {
        foreach (var group in GetAllGroups())
        {
            if (group.number == groupNumber)
            {
                return group;
            }
        }

        return null;
    }

    public static void InitializeAllGroups()
    {
        for (int i = 1; i < 10; i++)
        {
            AllGroups.Add(new Group(i));
        }
    }
    
    public Group(int number)
    {
        this.number = number;
        this.units = new List<Unit>();
    }
    
    public Group(int number, List<Unit> units)
    {
        this.number = number;
        this.units = new List<Unit>();
        foreach (var unit in units)
        {
            AddUnit(unit);
        }
    }

    public void AddUnit(Unit unit)
    {
        if (unit == null)
        {
            throw new Exception("Unit cannot be null");
        }else if (!unit.Groups.ContainsKey(number))
        {
            unit.Groups[number] = this;
        }

        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        if (unit == null)
        {
            throw new Exception("Unit cannot be null");
        }else if (unit.Groups.ContainsKey(number))
        {
            unit.Groups.Remove(number);
        }

        if (units.Contains(unit))
        {
            units.Remove(unit);
        }
    }

    public int Number
    {
        get => number;
        set => number = value;
    }

    public ReadOnlyCollection<Unit> Units
    {
        get
        {
            return new ReadOnlyCollection<Unit>(units);
        }
    }
}
