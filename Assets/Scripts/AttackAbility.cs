using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility
{
    public AttackAbility(int range, int attackPower, float attackDelay)
    {
        this.range = range;
        this.attackPower = attackPower;
        this.attackDelay = attackDelay;
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
