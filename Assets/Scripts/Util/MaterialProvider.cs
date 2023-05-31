using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialProvider : MonoBehaviour
{
    [SerializeField]
    Material glowMaterial;
    public Material GlowMaterial
    {
        get { return glowMaterial; }
    }

    private static MaterialProvider instance;
    public static MaterialProvider Instance { get { 
            return instance; } 
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
