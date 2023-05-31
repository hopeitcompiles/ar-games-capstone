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
        GameManager.Instance.OnMainMenu += Instance_OnMainMenu;
       
    }

    private void Instance_OnMainMenu()
    {
        gameObject.SetActive(false);
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
            rb.isKinematic = true;
            rb.useGravity = true;
        }
    }

}