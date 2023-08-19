using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Chomper : EnemyBase
{
    public WeaponAttackController weapon;

    public override void Attack()
    {
        base.Attack();
        animator.SetTrigger("Attack");
    }


    public void AttackBegin()
    {
        weapon.BeginAttack();
    }

    public void AttackEnd()
    {
        weapon.EndAttack();
    }
}