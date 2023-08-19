using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour
{
    #region 字段

    public float checkDistance; //检测距离
    public LayerMask layerMask; //检测层级
    public float maxHeihtDiff; //最大高度差

    [Range(0, 180)] public float lookAngle; //视野范围
    public GameObject target; //目标
    protected NavMeshAgent meshAgent; //导航代理
    public float followDistance; //追踪距离
    public float attackDistance; //攻击距离
    protected Vector3 startPosition; //开始位置
    protected RaycastHit[] results = new RaycastHit[10];
    
    protected Animator animator;
    public float walkSpeed = 2;
    public float runSpeed = 4;
    protected float moveSpeed;
    
    protected Rigidbody m_rigidbody;
    protected bool isCanAttack = true;
    public float attackTime; //攻击时间间隔
    protected float attackTimer; //攻击计时器
    #endregion

    #region 生命周期函数
    protected virtual void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        CheckTarget();
        FollowTarget();

        if (!isCanAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime)
            {
                isCanAttack = true;
                attackTimer = 0;
            }
        }
    }

    protected virtual void OnAnimatorMove()
    {
        m_rigidbody.MovePosition(m_rigidbody.position + animator.deltaPosition); //根运动
    }

    //绘制视野范围
    protected virtual void OnDrawGizmosSelected()
    {
        //画出检测范围
        Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 1.0f);
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        //最大高度差
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxHeihtDiff);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * maxHeihtDiff);

        //画出追踪范围
        Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 1.0f);
        Gizmos.DrawWireSphere(transform.position, followDistance);
        //画出攻击范围
        Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 1.0f);
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        //画出视野范围
        UnityEditor.Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.2f);
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, lookAngle, checkDistance);
        UnityEditor.Handles.DrawSolidArc(transform.position, -Vector3.up, transform.forward, lookAngle, checkDistance);
    }

    #endregion


    #region 方法

    //检测目标
    public virtual void CheckTarget()
    {
        int count = Physics.SphereCastNonAlloc(transform.position, checkDistance, Vector3.forward,
            results, 0, layerMask.value);
        for (int i = 0; i < count; i++)
        {
            //判断是不是可攻击的游戏物体
            if (results[i].transform.GetComponent<Damageable>() == null)
            {
                continue;
            }

            //判断高度差
            if (Mathf.Abs(results[i].transform.position.y - transform.position.y) > maxHeihtDiff)
            {
                continue;
            }

            //判断是不是在视野范围内
            if (Vector3.Angle(transform.forward, results[i].transform.position - transform.position) > lookAngle)
            {
                continue;
            }

            //找到目标(找到离自己最近的目标)
            if (target != null)
            {
                //判断一下距离
                float currentDistance = Vector3.Distance(transform.position, results[i].transform.position);
                float distance = Vector3.Distance(transform.position, target.transform.position); //上一次检测的距离
                if (currentDistance < distance)
                {
                    target = results[i].transform.gameObject;
                }
            }
            else
            {
                target = results[i].transform.gameObject;
            }
        }
    }

    //向目标移动的方法
    public virtual void MoveToTarget()
    {
        if (target == null)
            return;
        meshAgent.SetDestination(target.transform.position);
    }
    
    //转向看向目标
    public void ChangeDirection()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;
           // transform.rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
    }

    //追踪目标
    public virtual void FollowTarget()
    {
        //监听速度
        ListenerSpeed();

        if (target != null)
        {
            try
            {
                //向目标移动
                MoveToTarget();
                //转向看向目标
                ChangeDirection();
                //判断路径是否有效，是否能够到达
                if (meshAgent.pathStatus == NavMeshPathStatus.PathPartial ||
                    meshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    //目标丢失
                    LostTarget();
                    return;
                }

                //是否在可追踪距离内
                if (Vector3.Distance(transform.position, target.transform.position) > followDistance)
                {
                    //目标丢失
                    LostTarget();
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
            }
            catch (Exception e)
            {
                //追踪出错，目标丢失
                LostTarget();
            }
        }
    }

    //目标丢失
    public virtual void LostTarget()
    {
        target = null;
        // 回到初始位置
        meshAgent.SetDestination(startPosition);
        moveSpeed = walkSpeed;
    }

    public virtual void ListenerSpeed()
    {
        if (target != null)
        {
            moveSpeed = runSpeed;
        }

        meshAgent.speed = moveSpeed;

        animator.SetFloat("Speed", meshAgent.velocity.magnitude);
    }

    public virtual void Attack()
    {
       
    }

    #endregion
}