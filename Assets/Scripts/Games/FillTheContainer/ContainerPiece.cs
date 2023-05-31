using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Touchable))]
public class ContainerPiece : MonoBehaviour
{
    private Rigidbody rb;
    private bool isCorrect;

    public bool IsCorrect
    {
        get { return isCorrect; }
        set { isCorrect = value; }
    }
    void Start()
    {
       
    }

    public void MakeItDrop(bool state)
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<SphereCollider>();
            LeanDragTranslate leanDrag = gameObject.AddComponent<LeanDragTranslate>();
            leanDrag.Camera = FindAnyObjectByType<Camera>();
            rb.freezeRotation = true;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        GetComponent<Rigidbody>().isKinematic = !state;
        Debug.Log("changed"+state.ToString());
    }

}
