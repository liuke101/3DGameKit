using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //播放特效
        GameObject obj = animator.transform.GetComponent<Grenadier>().shootPosition.Find("GrenadeForm").gameObject;
        obj.SetActive(false);
        obj.SetActive(true);
        //创建一个子弹
        animator.transform.GetComponent<Grenadier>().CreateBullet();
    }
}