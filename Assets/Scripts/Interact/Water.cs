using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public DamageMessage damageMessage;
    public LayerMask layerMask;
    private void OnTriggerEnter(Collider other)
    {
        //判断是不是对应的层级
        if ((layerMask.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }
        
        //层级
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnDamage(damageMessage);
        }
    }
}
