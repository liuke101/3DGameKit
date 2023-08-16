using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Renderer[] renderers;

    public float dissolveTime = 3f;
    public float dissolveTimer = 0;
    private MaterialPropertyBlock m_propertyBlock;
    // Start is called before the first frame update
    void Start()
    {
        renderers = transform.GetComponentsInChildren<Renderer>();
        m_propertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        dissolveTimer += Time.deltaTime;
        
        for(int i =0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(m_propertyBlock);
            m_propertyBlock.SetFloat("_Cutoff", dissolveTimer / dissolveTime);
            renderers[i].SetPropertyBlock(m_propertyBlock);
        }
    }
}
