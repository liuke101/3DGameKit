using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllenAttackSMB : StateMachineBehaviour
{
    public int index;
    private Transform m_attackEffect;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().ShowWeapon(); //攻击时武器显示
        m_attackEffect = animator.transform.Find("TrailEffect/Ellen_Staff_Swish0" + index);
        m_attackEffect.gameObject.SetActive(false);
        m_attackEffect.gameObject.SetActive(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().HideWeapon(); //攻击结束时武器隐藏
        m_attackEffect = animator.transform.Find("TrailEffect/Ellen_Staff_Swish0" + index);
        m_attackEffect.gameObject.SetActive(false);
    }
    
    
}
