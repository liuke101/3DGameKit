using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region 字段
    private int m_currentIndex;
    private Action<float> m_onProgressChange;
    private Action m_onFinish;
    private static SceneController s_Instance;
    #endregion

    #region 属性
    public static SceneController Instance
    {
        get
        {
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("SceneController");
                s_Instance = obj.AddComponent<SceneController>();
            }
            return s_Instance;
        }
    }
    #endregion

    #region 生命周期

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(s_Instance!=null)
            throw new Exception("场景中存在多个SceneController实例");
        s_Instance = this;
    }

    #endregion
    
    #region 方法
    public void LoadScene(int index, Action<float> onProgressChange, Action onFinish)
    {
        m_currentIndex = index;
        m_onProgressChange = onProgressChange;
        m_onFinish = onFinish;
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(m_currentIndex);
        while (!asyncOperation.isDone)
        {
            yield return null;
            m_onProgressChange?.Invoke(asyncOperation.progress); //加载进度
        }

        yield return new WaitForSeconds(1f);
        m_onFinish?.Invoke();
    }
    #endregion
    
}
