using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Spitter : Chomper
{
    public float escapeDistance; //逃跑距离
    public GameObject bulletPrefab;

    public void Escape()
    {
        animator.ResetTrigger("Attack");
        meshAgent.isStopped = false;
        meshAgent.speed = moveSpeed;
        Vector3 dir = transform.position - target.transform.position;
        Vector3 targetPos = transform.position + dir.normalized;
        meshAgent.SetDestination(targetPos);
    }

    public override void FollowTarget()
    {
        //监听速度
        ListenerSpeed();
        
        if (target != null)
        {
            try
            {
                //判断要不要逃跑
                if (Vector3.Distance(transform.position, target.transform.position) <= escapeDistance)
                {
                    //逃跑
                    Escape();
                    return;
                }
                
                //向目标移动
                MoveToTarget();
                //转向看向目标
                ChangeDirection();
                //判断路径是否有效，是否能够到达
                if (meshAgent.pathStatus == NavMeshPathStatus.PathPartial ||
                    meshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
                    {
                        //目标丢失
                        LoseTarget();
                        return;
                    }
                }

                //是否在可追踪距离内
                if (Vector3.Distance(transform.position, target.transform.position) > followDistance)
                {
                    LoseTarget();
                    return;
                }

                //是不是在攻击范围内
                if (Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
                {
                    if (isCanAttack)
                    {
                        Attack();
                        isCanAttack = false;
                    }
                }

                if (!target.transform.GetComponent<Damageable>().IsAlive)
                {
                    LoseTarget();
                    return;
                }
            }
            catch (System.Exception)
            {
                //追踪出错，目标丢失
                LoseTarget();
            }
        }
    }
    public override void MoveToTarget()
    {
        base.MoveToTarget();
        
        //到达攻击范围内停止移动
        if(Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
        {
            meshAgent.isStopped = true;
        }
        else
        {
            meshAgent.isStopped = false;
        }
    }

    protected override void OnAnimatorMove()
    {
        //攻击时不需要移动
    }
    
    public override void AttackBegin()
    {
        //创建一个子弹
        GameObject bullet = Instantiate(bulletPrefab, transform.Find("ShootPoint").transform.position, Quaternion.identity);
        if (bullet != null)
        {
            bullet.GetComponent<SpitterBullet>().Shoot(target.transform.position, transform.forward);
        }
    }

    public override void AttackEnd()
    {
        
    }

    // protected override void OnDrawGizmosSelected()
    // {
    //     base.OnDrawGizmosSelected();
    //     Handles.color = new Color(Color.cyan.r,Color.cyan.g,Color.cyan.b, 0.1f);
    //     Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, 360, escapeDistance);
    // }
}
