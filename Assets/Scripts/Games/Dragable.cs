using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    public static readonly string dragTag = "drag";
    private bool isSelected;

    void Start()
    {
        gameObject.tag = dragTag;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("from dragable");
    }
}
