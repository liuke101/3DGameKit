using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterBullet : Bullet
{
    public override void Shoot(Vector3 target, Vector3 direction)
    {
        base.Shoot(target, direction);
        //抛物线运动
        float g = Mathf.Abs(Physics.gravity.y); //重力加速度
        float v0 = 8; //竖直向上的初速度
        float t0 = v0 / g;
        float y0 = 0.5f * g * t0 * t0;

        float t = 0;

        if (transform.position.y + y0 > target.y)
        {
            float t1 = Mathf.Sqrt(2 * (y0 + (transform.position.y - target.y)) / g);
            t = t0 + t1;
        }
        else
        {
            t = t0;
        }

        Vector3 transPos = transform.position;
        transPos.y = 0;
        target.y = 0;

        float speed = Vector3.Distance(transPos, target) / t;
        Vector3 velocity = direction.normalized * speed + Vector3.up * v0;
        m_rigidbody.isKinematic = false;
        m_rigidbody.velocity = velocity;
    }

}