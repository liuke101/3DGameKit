using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadView : ViewBase
{
    #region 字段
    public Slider progressSlider;
    public TMP_Text progressText;
    #endregion
   

    #region 生命周期
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    #endregion


    #region 方法

    public void UpdateProgress(float progress)
    {
        progressSlider.value = progress;
    }
    
    public void OnSliderProgressValueChanged(float value)
    {
        progressText.text = $"{Mathf.Round(value * 100)}%";
    }

    #endregion
   
}