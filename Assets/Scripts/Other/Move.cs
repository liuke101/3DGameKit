using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveType
{
    Once,
    Loop,
    PingPong
}

public enum PositionType
{
    World,
    Local
}


public class Move : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    public float time = 1; //移动的时间
    private float m_timer; //移动的计时器
    private float m_percent; //移动的百分比
    private bool m_isMoving; //是否正在移动
    
    public MoveType moveType = MoveType.Once;
    public UnityEvent onMoveEnd;
    
    public PositionType positionType = PositionType.Local;
    public bool moveOnAwake = false;
    
    public float delayTime = 0;
    private float m_delayTimer = 0;

    private void Awake()
    {
        if (moveOnAwake)
        {
            StartMove();
        }
    }

    void Update()
    {
        if (m_isMoving)
        {
            CalculateMove();
        }
    }

    public void StartMove()
    {
        m_isMoving = true;    
        m_timer = 0;
    }
    
    private void CalculateMove()
    {
        if(m_delayTimer<delayTime)
        {
            m_delayTimer += Time.deltaTime;
            return;
        }
        
        m_timer += Time.deltaTime/time;
        
        switch (moveType)
        {
            case MoveType.Once:
                m_percent = Mathf.Clamp01(m_timer);
                break;
            case MoveType.Loop:
                m_percent = Mathf.Repeat(m_timer,1);
                break;
            case MoveType.PingPong:
                m_percent = Mathf.PingPong(m_timer,1);
                break;
        }

        MoveExcute();

        if (m_timer >= 1 && moveType == MoveType.Once)
        {
            m_isMoving = false;
            m_timer = 0;
            //移动结束了
            onMoveEnd?.Invoke();
        }
    }

    private void MoveExcute()
    {
        switch (positionType)
        {
            case PositionType.World:
                transform.position = Vector3.Lerp(startPosition,endPosition,m_percent);
                break;
            case PositionType.Local:
                transform.localPosition = Vector3.Lerp(startPosition,endPosition,m_percent);
                break;
        }
    }
}
