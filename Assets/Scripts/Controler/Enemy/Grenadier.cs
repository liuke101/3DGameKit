using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grenadier : EnemyBase
{
    #region 字段
    public float shortAttackDistance;
    private int m_shortAttackHash = Animator.StringToHash("Grenadier_MeleeAttack");
    private int m_rangeAttackHash = Animator.StringToHash("Grenadier_RangeAttack");
    private int m_rangeAttack2Hash = Animator.StringToHash("Grenadier_RangeAttack2");
    private AnimatorStateInfo m_currentStateInfo; //当前动画的信息
    public GameObject balletPrefab;
    public Transform shootPosition;
    private Bullet m_bullet;
    public WeaponAttackController meleeAttackController;
    public WeaponAttackController rangeAttackController;
    public float rangeAttackTriggerAngle;
    public Transform rangeAttackEffectPosition;
    public GameObject rangeAttackEffectCharge;
    public GameObject rangeAttackEffectShield;
    #endregion

    #region 生命周期
    protected override void Update()
    {
        base.Update();
        m_currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
    

    #endregion
    
    #region 方法
    public override void Attack()
    {
        base.Attack();

        //如果正在播放攻击动画，不重复播放
        if (m_currentStateInfo.shortNameHash == m_shortAttackHash ||
            m_currentStateInfo.shortNameHash == m_rangeAttackHash ||
            m_currentStateInfo.shortNameHash == m_rangeAttack2Hash)
        {
            animator.ResetTrigger("Attack_Short");
            animator.ResetTrigger("Attack_Range");
            animator.ResetTrigger("Attack_Shoot");
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) > shortAttackDistance)
        {
            Turn();
            //远距离攻击
            Debug.Log("远距离攻击");
            animator.ResetTrigger("Attack_Shoot");
            animator.SetTrigger("Attack_Shoot");
        }
        else
        {
            Turn();
            //近距离攻击
            if (Vector3.Angle(transform.forward, target.transform.position - transform.position) > rangeAttackTriggerAngle)
            {
                Debug.Log("近距离范围攻击");
                animator.ResetTrigger("Attack_Range");
                animator.SetTrigger("Attack_Range");
            }
            else
            {
                Debug.Log("近距离普通攻击");
                animator.ResetTrigger("Attack_Short");
                animator.SetTrigger("Attack_Short");
            }
        }
    }

    protected override void OnAnimatorMove()
    {
        transform.rotation *= animator.deltaRotation;
    }

    public override void ChangeDirection()
    {
        //base.ChangeDirection();
    }

    //转弯
    public void Turn()
    {
        //计算角度
        float angle = Vector3.SignedAngle(transform.forward, target.transform.position - transform.position, Vector3.up);

        if (Mathf.Abs(angle) > 10)
        {
            animator.SetFloat("TurnAngle", angle);
            animator.SetTrigger("Turn");
        }
    }
    
    public void CreateBullet()
    {
        //生成子弹
        GameObject bullet = Instantiate(balletPrefab, shootPosition.position, Quaternion.identity);
        bullet.transform.parent = shootPosition;
        bullet.transform.localPosition = Vector3.zero;
        m_bullet = bullet.GetComponent<Bullet>();
    }
    #endregion
    
    #region 动画事件
    public void Shoot()
    {
        if(target!=null)
        {
            m_bullet.Shoot(target.transform.position, transform.forward);
        }
        else
        {
            Destroy(m_bullet.gameObject);
        }
        m_bullet = null;
    }
 
    public void MeleeAttackStart()
    {
        meleeAttackController.BeginAttack();
       
    }
    
    public void MeleeAttackEnd()
    {
       meleeAttackController.EndAttack();
    }
    
    public void RangeAttackBegin()
    {
        GameObject effectCharge = Instantiate(rangeAttackEffectCharge, rangeAttackEffectPosition.position, Quaternion.identity);
        Destroy(effectCharge, 2.0f);
    }
    
    public void RangeAttackStart()
    {
        rangeAttackController.BeginAttack();
        GameObject effectShield = Instantiate(rangeAttackEffectShield, rangeAttackEffectPosition.position, Quaternion.identity);
        Destroy(effectShield, 2.0f);
    }
    
    public void RangeAttackEnd()
    {
        rangeAttackController.EndAttack();
    }
    
    #endregion
    
   
}