using System;
using System.Collections;
using System.Collections.Generic;
using Gamekit3D;
using UnityEngine;

[Serializable]
public class CheckPoint
{
    public Transform point;
    public float radius;
}

public class WeaponAttackController : MonoBehaviour
{
    #region 字段
    public CheckPoint[] checkPoint;
    public LayerMask layerMask;
    private RaycastHit[] m_results = new RaycastHit[10];
    private bool m_isAttack = false;

    public int damage;
    public GameObject mySelf;
    private List<GameObject> m_attackList = new List<GameObject>();
    public GameObject hitPrefab;
    #endregion


    #region 生命名周期函数
    void Start()
    {
    }

    void Update()
    {
        CheckGameObject();
    }
    #endregion


    #region 方法
    public void BeginAttack()
    {
        m_isAttack = true;
    }

    public void EndAttack()
    {
        m_isAttack = false;
        m_attackList.Clear();
    }
    

    //检测敌人
    public void CheckGameObject()
    {
        if (!m_isAttack)
            return;

        for (int i = 0; i < checkPoint.Length; i++)
        {
            int count = Physics.SphereCastNonAlloc(checkPoint[i].point.position, checkPoint[i].radius, Vector3.forward,
                m_results, 0, layerMask.value);
            for (int j = 0; j < count; j++)
            {
                if (CheckDamage(m_results[j].transform.gameObject))
                {
                    if (hitPrefab != null)
                    {
                        GameObject hit = Instantiate(hitPrefab, checkPoint[i].point.position, Quaternion.identity);
                        Destroy(hit, 1.0f);
                    }
                }
            }
        }
    }

    //对敌人造成伤害
    public bool CheckDamage(GameObject obj)
    {
        //判断游戏物体是不是有受伤的功能
        Damageable damageable = obj.GetComponent<Damageable>();

        //以下情况不进行攻击
        //对象如果没有受伤功能|检测到自己|已经攻击过的对象|
        if (damageable==null || obj == mySelf || m_attackList.Contains(obj))
            return false;

        //进行攻击
        DamageMessage message = new DamageMessage();
        message.damage = damage;
        message.damagePosition = mySelf.transform.position;
        damageable.OnDamage(message);
        m_attackList.Add(obj);
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if(checkPoint==null)
            return;
        
        for (int i = 0; i < checkPoint.Length; i++)
        {
            if(checkPoint[i]==null)
                continue;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(checkPoint[i].point.position, checkPoint[i].radius);
        }
    }
    #endregion
   
}