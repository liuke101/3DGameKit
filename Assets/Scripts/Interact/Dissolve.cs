using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Renderer[] renderers;

    public float dissolveTime = 3f;
    public float dissolveTimer = 0;
    private MaterialPropertyBlock m_propertyBlock;

    private void OnEnable()
    {
        dissolveTimer = 0;
    }

    void Start()
    {
        renderers = transform.GetComponentsInChildren<Renderer>();
        m_propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        dissolveTimer += Time.deltaTime;

        if (dissolveTimer >= dissolveTime)
            return;
        
        for(int i =0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(m_propertyBlock);
            m_propertyBlock.SetFloat("_Cutoff", dissolveTimer / dissolveTime);
            renderers[i].SetPropertyBlock(m_propertyBlock);
        }
    }
}
