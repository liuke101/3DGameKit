using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    private Vector2 m_move;
    private bool m_jump;
    private bool m_attack;
    private bool isCanControl = true;
    private bool m_pause;
    
    public Vector2 Move
    {
        get
        {
            if(!isCanControl)
                return Vector2.zero;
            
            return m_move;
        }
    }
    public bool Jump => m_jump&&isCanControl;
    public bool Attack => m_attack&&isCanControl;

    public bool IsCanControl => isCanControl;
    public bool Pause => m_pause&&isCanControl;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        m_move.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_jump = Input.GetButton("Jump");
        m_attack = Input.GetButton("Fire1");
        m_pause = Input.GetButtonDown("Pause");
    }
    
    //获得控制
    public void GainControl()
    {
        isCanControl = true;
    }
    
    //失去控制
    public void ReleaseControl()
    {
        isCanControl = false;
    }

}
