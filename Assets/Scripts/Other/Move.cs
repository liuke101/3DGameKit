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
    protected float timer; //移动的计时器
    protected float percent; //移动的百分比
    protected bool isMoving; //是否正在移动
    
    public MoveType moveType = MoveType.Once;
    public UnityEvent onMoveEnd;
    
    public PositionType positionType = PositionType.Local;
    public bool moveOnAwake = false;
    
    public float delayTime = 0;
    protected float delayTimer = 0;

    protected void Awake()
    {
        if (moveOnAwake)
        {
            StartMove();
        }
    }

    protected void Update()
    {
        if (isMoving)
        {
            CalculateMove();
        }
    }

    public void StartMove()
    {
        isMoving = true;    
        timer = 0;
    }
    
    protected void CalculateMove()
    {
        if(delayTimer<delayTime)
        {
            delayTimer += Time.deltaTime;
            return;
        }
        
        timer += Time.deltaTime/time;
        
        switch (moveType)
        {
            case MoveType.Once:
                percent = Mathf.Clamp01(timer);
                break;
            case MoveType.Loop:
                percent = Mathf.Repeat(timer,1);
                break;
            case MoveType.PingPong:
                percent = Mathf.PingPong(timer,1);
                break;
        }

        MoveExcute();

        if (timer >= 1 && moveType == MoveType.Once)
        {
            isMoving = false;
            timer = 0;
            //移动结束了
            onMoveEnd?.Invoke();
        }
    }

    protected virtual void MoveExcute()
    {
        switch (positionType)
        {
            case PositionType.World:
                transform.position = Vector3.Lerp(startPosition,endPosition,percent);
                break;
            case PositionType.Local:
                transform.localPosition = Vector3.Lerp(startPosition,endPosition,percent);
                break;
        }
    }
}
