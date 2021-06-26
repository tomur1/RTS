using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnityEngine;

public class AttackAbility
{
    private Soldier soldier;

    public Soldier Soldier => soldier;

    private AttackAbility(int range, int attackPower, float attackDelay, Soldier soldier)
    {
        AssignSoldier(soldier);
        if (range < 1)
        {
            throw new Exception("Range cannot be less than 1");
        }
        this.range = range;
        if (attackPower <= 0)
        {
            throw new Exception("Attack Power cannot be zero or less");
        }
        this.attackPower = attackPower;
        if (attackDelay <= 0)
        {
            throw new Exception("Attack Delay cannot be zero or less");
        }
        this.attackDelay = attackDelay;
    }

    private void AssignSoldier(Soldier soldier)
    {
        if (soldier == null) {
            throw new Exception("Soldier cannot be null");
        }
            
        var oldAbility = soldier.AttackAbility;
        if (oldAbility != null)
        {
            oldAbility.RemoveElement();
        }

        soldier.AttackAbility = this;
        this.soldier = soldier;
    }

    private void RemoveElement()
    {
        soldier = null;
    }

    public static void CreateAndAssign(int range, int attackPower, float attackDelay, Soldier soldier)
    {
        new AttackAbility(range, attackPower, attackDelay, soldier);
    }

    public float GetAttackSpeed()
    {
        return 1 / attackDelay;
    }

    public int range;
    public int attackPower;
    //Time to wait for each attack in seconds
    public float attackDelay;
}
