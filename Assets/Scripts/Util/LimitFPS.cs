using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    public static LimitFPS instance;

    private void Awake()
    {
        instance = this;
    }
    public void TargetFrameRate(int value)
    {
         Application.targetFrameRate = value; 
    }

}
