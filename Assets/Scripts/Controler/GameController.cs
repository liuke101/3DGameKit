using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isShowCursor = false;

    public ViewBase pauseView;
    public void Awake()
    {
        ShowCursor(isShowCursor);
    }

    private void Update()
    {
        if (PlayerInput.instance != null && PlayerInput.instance.Pause)
        {
            Pause(true);
        }
    }

    public void Pause(bool isPause)
    {
        //显示鼠标
        ShowCursor(isPause);
        //让玩家失去控制
        if (isPause&&PlayerInput.instance!=null)
        {
            PlayerInput.instance.ReleaseControl();
        }
        else
        {
            PlayerInput.instance.GainControl();
        }
        //停止游戏逻辑
        Time.timeScale = isPause ? 0 : 1;
        //显示暂停界面
        if (isPause)
        {
            pauseView.Show();
        }
        else
        {
            pauseView.Hide();
        }
       
    }
    public void ShowCursor(bool isShow)
    {
        //隐藏鼠标
        Cursor.visible = isShow;
        //锁定鼠标
        Cursor.lockState = isShow?CursorLockMode.None:CursorLockMode.Locked;
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReStart()
    {
        Pause(false);
        transform.GetComponent<SceneChange>().Change();
    }
}