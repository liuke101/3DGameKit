using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 m_move;
    private bool m_jump;
    private bool m_attack;
    
    public Vector2 Move => m_move;
    public bool Jump => m_jump;
    public bool Attack => m_attack;

    private void Update()
    {
        m_move.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_jump = Input.GetButton("Jump");
        m_attack = Input.GetButton("Fire1"); 
    }
}
