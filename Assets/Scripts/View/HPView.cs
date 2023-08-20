using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HPView : ViewBase
{
    public GameObject hpItemPrefab;
    public Damageable damageable;

    private Toggle[] m_hps;

    private void Start()
    {
        InitialHp();
    }

    public void UpdateHPView()
    {
        for (int i = 0; i < m_hps.Length; i++)
        {
            //播放动画
            if (m_hps[i].isOn && i >= damageable.currentHp)
            {
                m_hps[i].transform.Find("Background/HpDissolve").gameObject.SetActive(false);
                m_hps[i].transform.Find("Background/HpDissolve").gameObject.SetActive(true);
            }

            //更新血条
            m_hps[i].isOn = i < damageable.currentHp;
        }
    }

    public void InitialHp()
    {
        StartCoroutine(InitialHpView());
    }

    public IEnumerator InitialHpView()
    {
        //血条初始化
        if (m_hps != null)
        {
            for (int i = 0; i < m_hps.Length; i++)
            {
                Destroy(m_hps[i].gameObject);
            }
        }
        
        m_hps = new Toggle[damageable.maxHp];
        yield return null;
        for (int i = 0; i < damageable.maxHp; i++)
        {
            yield return new WaitForSeconds(0.01f);
            GameObject hpItem = Instantiate(hpItemPrefab, transform.Find("Hps"));
            m_hps[i] = hpItem.GetComponent<Toggle>();
        }
    }
}