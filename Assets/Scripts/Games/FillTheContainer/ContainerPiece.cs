using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Touchable))]
public class ContainerPiece : MonoBehaviour, IPiece
{
    private Rigidbody rb;
    private bool isCorrect;

    public bool IsCorrect
    {
        get { return isCorrect; }
        set { isCorrect = value; }
    }

    private void Instance_OnMainMenu()
    {
        gameObject.SetActive(false);
    }
    public void SetUp(int layer)
    {
        //gameObject.GetComponent<MeshCollider>().gameObject.layer = layer;
        GameManager.Instance.OnMainMenu += Instance_OnMainMenu;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic= true;
        rb.useGravity= true;
        GetComponent<MeshCollider>().convex = true; 
        GetComponent<MeshCollider>().isTrigger = true;
        gameObject.AddComponent<ObjectManipulator>();
    }
}
