using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    public float time;//子弹飞行时间
    private Rigidbody m_rigidbody;
    public GameObject explosionEffect;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    
    public void Shoot(Vector3 target,Vector3 direction)
    {
        transform.SetParent(null); //解除父子关系,防止刚体受到父物体的影响
        m_rigidbody.isKinematic = false;
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0;
        
        float speed = toTarget.magnitude/ time;
        m_rigidbody.velocity = direction.normalized * speed+Vector3.up*3.0f;
        Invoke("Attack",time);
    }

    public void Attack()
    {
        //爆炸
        Explosion();
        //对人物进行攻击
        transform.GetComponent<WeaponAttackController>().BeginAttack();
        
    }

    public void Explosion()
    {
        if(explosionEffect!=null)
        {
            GameObject expolision = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(expolision, 2.0f);
        }

        Destroy(gameObject, 0.2f);
    }
}
