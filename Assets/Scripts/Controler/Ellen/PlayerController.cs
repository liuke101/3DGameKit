using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    #region 字段
    public CharacterController characterController;
    public Transform mainCamera;

    public float maxMoveSpeed = 5f;
    public float moveSpeed;
    public float maxAngleSpeed = 1200f;
    public float minAngleSpeed = 400f;
    public float accelerateSpeed = 5f;
    public float jumpSpeed = 10f;
    public float gravity = 20f;
    public bool isGrounded = true; //默认在地面上
    public bool isCanAttack;
    public GameObject weapon;

    private float m_verticalSpeed;
    private PlayerInput m_playerInput;
    private Vector3 m_move;

    private Animator m_animator;
    private AnimatorStateInfo m_currentStateInfo;
    private AnimatorStateInfo m_nextStateInfo;
    private int m_quickTurnLeftHash = Animator.StringToHash("Ellen_QuickTurnLeft");
    private int m_quickTurnRightHash = Animator.StringToHash("Ellen_QuickTurnRight");
    #endregion

    #region 生命周期函数

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        m_playerInput = GetComponent<PlayerInput>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        m_currentStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        m_nextStateInfo = m_animator.GetNextAnimatorStateInfo(0);


        CalculateVerticalSpeed();
        CalculateForward();
        CalculateRotation();

        m_animator.SetFloat("normalizedTime", Mathf.Repeat(m_currentStateInfo.normalizedTime,1));
        m_animator.ResetTrigger("Attack");
        if (m_playerInput.Attack && isCanAttack)
        {
            m_animator.SetTrigger("Attack");
        }
    }

    private void OnAnimatorMove()
    {
        CalculateMove();
    }
    #endregion

    #region 方法
    public void CalculateMove()
    {
        if (isGrounded)
        {
            m_move = m_animator.deltaPosition; //通过动画根运动计算移动，无法在空中运动
        }
        else
        {
            m_move = moveSpeed * transform.forward * Time.deltaTime; //空中移动
        }

        m_move += Vector3.up * (m_verticalSpeed * Time.deltaTime); //跳跃
        transform.rotation *= m_animator.deltaRotation; //通过动画根运动计算旋转
        characterController.Move(m_move);
        isGrounded = characterController.isGrounded;

        m_animator.SetBool("isGrounded", isGrounded);
    }

    public void CalculateVerticalSpeed()
    {
        if (isGrounded)
        {
            m_verticalSpeed = -gravity * 0.3f;
            if (m_playerInput.Jump)
            {
                m_verticalSpeed = jumpSpeed;
                isGrounded = false;
            }
        }
        else
        {
            //按住空格键，跳跃高度增加
            if (!m_playerInput.Jump && m_verticalSpeed > 0)
            {
                m_verticalSpeed -= gravity * Time.deltaTime;
            }

            m_verticalSpeed -= gravity * Time.deltaTime;
        }
        
        if(!isGrounded)
            m_animator.SetFloat("verticalSpeed", m_verticalSpeed);
    }

    private void CalculateForward()
    {
        //移动速度逐渐加快
        moveSpeed = Mathf.MoveTowards(moveSpeed, maxMoveSpeed * m_playerInput.Move.normalized.magnitude,
            accelerateSpeed * Time.deltaTime);
        m_animator.SetFloat("forwardSpeed", moveSpeed);
    }

    private void CalculateRotation()
    {
        if (m_playerInput.Move.x != 0 || m_playerInput.Move.y != 0)
        {
            Vector3 targetDirection =
                mainCamera.TransformDirection(new Vector3(m_playerInput.Move.x, 0, m_playerInput.Move.y));
            targetDirection.y = 0;

            //根据移动速度调整转向速度，移动速度越快，转向速度越慢
            float turnSpeed = Mathf.Lerp(maxAngleSpeed, minAngleSpeed, moveSpeed / maxMoveSpeed) * Time.deltaTime;
            //使用带符号旋转角度可以判断左右
            float turnAngle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

            //如果没有播放快速转身动画，使用程序计算转向，否则使用根运动计算转向
            if (IsUpdateDirection())
            {
                //逐渐转向
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation(targetDirection),
                    turnSpeed);
            }


            m_animator.SetFloat("turnRadian", turnAngle * Mathf.Deg2Rad);
        }
    }

    public bool IsUpdateDirection()
    {
        bool isUpdate = m_currentStateInfo.shortNameHash != m_quickTurnLeftHash &&
                        m_currentStateInfo.shortNameHash != m_quickTurnRightHash &&
                        m_nextStateInfo.shortNameHash != m_quickTurnLeftHash &&
                        m_nextStateInfo.shortNameHash != m_quickTurnRightHash;
        return isUpdate;
    }
    
    public void SetCanAttack(bool isAttack)
    {
        this.isCanAttack = isAttack;
    }

    public void ShowWeapon()
    {
        CancelInvoke("HideWeaponExcute");
        weapon.SetActive(true);
    }
    
    public void HideWeapon()
    {
        Invoke("HideWeaponExcute",1); //延迟1秒隐藏武器,防止连击动画播放时武器消失
    }
    private void HideWeaponExcute()
    {
        weapon.SetActive(false);
    }
    
    public void OnHurt(Damageable damageable,DamageMessage message)
    {
        //计算受击方向
        Vector3 direction = message.damagePosition - transform.position;
        direction.y = 0;
        Vector3 localDirection = transform.InverseTransformDirection(direction); //转换为局部坐标
        m_animator.SetFloat("HurtX",localDirection.x);
        m_animator.SetFloat("HurtY",localDirection.z);
        m_animator.SetTrigger("Hurt");
    }
    #endregion

    #region 动画事件
    public void OnIdleStart()
    {
        m_animator.SetInteger("RadomIdle", -1);
    }

    public void OnIdleEnd()
    {
        m_animator.SetInteger("RadomIdle", Random.Range(0, 3));
    }
    
    public void MeleeAttackStart()
    {
        weapon.GetComponent<WeaponAttackController>().BeginAttack();
    }
    
    public void MeleeAttackEnd()
    {
        weapon.GetComponent<WeaponAttackController>().EndAttack();
    }
    #endregion
}