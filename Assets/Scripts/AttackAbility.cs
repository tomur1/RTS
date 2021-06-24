using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : MonoBehaviour
{
    public AttackAbility(int range, int attackPower, float attackDelay, float attackType)
    {
        this.range = range;
        this.attackPower = attackPower;
        this.attackDelay = attackDelay;
        this.attackType = attackType;
    }

    public int range;
    public int attackPower;
    //Time to wait for each attack in seconds
    public float attackDelay;
    public float attackType;
}
