using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterial : MonoBehaviour
{
   public Material[] target;
   private Renderer m_renderer;

   private void Awake()
   {
      m_renderer = GetComponent<Renderer>();
      
      if(m_renderer == null)
         throw new Exception("Renderer is null");
   }

   public void Switch()
   {
     m_renderer.materials = target;
   }
}
