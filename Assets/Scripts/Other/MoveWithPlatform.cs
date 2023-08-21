using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlatform : Move
{
    public CharacterController m_characterController; //玩家控制器
    private Vector3 m_deltaPos; //平台移动的距离

    private void OnTriggerEnter(Collider other)
    {
        m_characterController = other.transform.GetComponent<CharacterController>();
    }

    private void OnTriggerExit(Collider other)
    {
        m_characterController = null;
    }

    protected override void MoveExcute()
    {
        switch (positionType)
        {
            case PositionType.World:
                m_deltaPos = Vector3.Lerp(startPosition,endPosition,percent) - transform.position;
                break;
            case PositionType.Local:
                m_deltaPos = Vector3.Lerp(startPosition,endPosition,percent)- transform.localPosition;
                break;
        }
        
        base.MoveExcute();
        
        if (m_characterController != null)
        {
            m_characterController.Move(m_deltaPos);
        }
    }
}
