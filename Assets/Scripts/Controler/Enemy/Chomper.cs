using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Chomper : EnemyBase
{
    public WeaponAttackController weapon;

    protected override void Start()
    {
        base.Start();
        animator.Play("Chomper_Idle",0, Random.Range(0.0f, 1.0f)); //让每个敌人动画不同
    }

    public override void Attack()
    {
        base.Attack();
        animator.SetTrigger("Attack");
    }


    public virtual void AttackBegin()
    {
        weapon.BeginAttack();
    }

    public virtual void AttackEnd()
    {
        weapon.EndAttack();
    }

    public override void OnDeath(Damageable damageable, DamageMessage message)
    {
        base.OnDeath(damageable, message);

        //丢失目标
        LoseTarget();
        //停止追踪
        meshAgent.isStopped = true;
        meshAgent.enabled = false;
        //播放死亡动画
        animator.SetTrigger("Death");
        //添加一个力，让他飞出去
        Vector3 force = transform.position - message.damagePosition;
        force.y = 0;
        m_rigidbody.isKinematic = false;
        m_rigidbody.AddForce(force.normalized * 8 + Vector3.up * 4, ForceMode.Impulse);

        //销毁
        Invoke("Dissolve",3.0f);
    }

    public void Dissolve()
    {
        transform.Find("Body").gameObject.SetActive(false);
        transform.Find("Body_Dissolve").gameObject.SetActive(true);
        Destroy(gameObject, 1.0f);
    }
}