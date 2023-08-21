using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum DoorStatus
{
    Open,
    Close
}
public class Door : MonoBehaviour
{
    
    public DoorStatus doorStatus = DoorStatus.Close;

    public int keyNum = 1; //需要的钥匙数量
    public int currentKeyNum = 0; //当前拥有的钥匙数量
    public UnityEvent onOpen;
    public UnityEvent onClose;
    
    public void Open()
    {
        currentKeyNum++;
        
        if (doorStatus == DoorStatus.Close && currentKeyNum==keyNum)
        {
            doorStatus = DoorStatus.Open;
            onOpen?.Invoke();
        }
    }

    public void DelayOpen(float delayOpenTime)
    {
        Invoke("Open", delayOpenTime);
    }
    
    public void Close()
    {
        if (doorStatus == DoorStatus.Open)
        {
            doorStatus = DoorStatus.Close;
            onClose?.Invoke();
        }
    }
}
