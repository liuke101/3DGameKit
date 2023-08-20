using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackMaskView : MonoBehaviour
{
    public static BlackMaskView Instance;
    private Image m_image;

    private void Awake()
    {
        if (Instance != null)
            throw new Exception("场景中存在多个BlackMaskView");
        Instance = this;
    }

    private void Start()
    {
        m_image = transform.GetComponent<Image>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    //渐入 透明度从1-0
    public IEnumerator FadeIn()
    {
        m_image.color = new Color(m_image.color.r,m_image.color.g,m_image.color.b,1);
        yield return null;

        while (m_image.color.a > 0)
        {
            yield return null;
            m_image.color = new Color(m_image.color.r,m_image.color.g,m_image.color.b,m_image.color.a - Time.deltaTime);
        }
    }
    
    //渐出 透明度从0-1
    public IEnumerator FadeOut()
    {
        m_image.color = new Color(m_image.color.r,m_image.color.g,m_image.color.b,0);
        yield return null;

        while (m_image.color.a < 1)
        {
            yield return null;
            m_image.color = new Color(m_image.color.r,m_image.color.g,m_image.color.b,m_image.color.a + Time.deltaTime);
        }
    }
    
}
