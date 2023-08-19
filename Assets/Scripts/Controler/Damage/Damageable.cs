using System;
using System.Collections;
using System.Collections.Generic;
using Gamekit3D;
using UnityEngine;
using UnityEngine.Events;

public class DamageMessage
{
    public int damage; //伤害
    public Vector3 damagePosition; //伤害来源
}

[Serializable]
public class DamageEvent : UnityEvent<Damageable,DamageMessage>
{
    
}

public class Damageable : MonoBehaviour
{
    #region 字段j
    public int maxHp; //最大血量
    public int currentHp; //当前血量
    
    public float invincibleTime; //无敌时间
    private bool m_isInvinible = false; //是否无敌
    private float m_invincibleTimer = 0; //无敌时间计时器
    
    public DamageEvent onHurt;
    public DamageEvent onDead;
    public DamageEvent onReset;
    public DamageEvent onInvicibleTimeOut;
    #endregion


    #region 生命周期函数
    private void Start()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (m_isInvinible)
        {
            m_invincibleTimer += Time.deltaTime;
            if (m_invincibleTimer >= invincibleTime)
            {
                m_isInvinible = false;
                m_invincibleTimer = 0;
                onInvicibleTimeOut?.Invoke(this,null);
            }
        }
            
    }
    #endregion
   
    
    #region 方法
    public void OnDamage(DamageMessage message)
    {
        if (currentHp <= 0)
            return;
        if(m_isInvinible) //无敌
            return;
        
        currentHp -= message.damage;
        m_isInvinible = true;

        //死亡
        if (currentHp <= 0)
        {
            onDead?.Invoke(this,message);
        }
        //受伤
        else
        {
            onHurt?.Invoke(this,message);
        }
    }

    public void ResetDamage()
    {
        currentHp = maxHp;
        m_isInvinible = false;
        m_invincibleTimer = 0;
        onReset?.Invoke(this,null);
    }
    #endregion
    
}
