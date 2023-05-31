using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Touchable))]
[RequireComponent(typeof(Data))]
public class PuzzlePiece : MonoBehaviour
{
    Rigidbody rb;
    Touchable touchable;
    public Touchable Touchable
    {
        get { return touchable; }
    }
    public int LayerMask
    {
        set
        {
            GetComponent<Collider>().gameObject.layer = value;
        }
    }

    void Awake()
    {
        touchable = GetComponent<Touchable>();
        
    }

    public void MakeItDrop(bool state)
    {
        if (rb == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
            gameObject.AddComponent<SphereCollider>();
            rb.freezeRotation = true;
            rb.isKinematic = true;
        }
        rb.isKinematic = !state; 
    }
    public void Move(Vector3 position)
    {
        transform.position = position;
    }
}
