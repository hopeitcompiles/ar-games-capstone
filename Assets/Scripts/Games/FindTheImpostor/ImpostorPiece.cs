using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Touchable))]
public class ImpostorPiece : MonoBehaviour
{
    [SerializeField]
    private int id; 
    [SerializeField]
    private ImpostorPiece faked;
    private Vector3 originalPosition;
    private int layerMask;
    private Touchable touchable;

    public Touchable Touchable { get { return touchable; } }
    public Vector3 OriginalPosition
    {
        get { return originalPosition; }
    }
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public ImpostorPiece Faked
    {
        get { return faked; }
        set { 
            faked = value;
        }
    }
    public void ChangePosition(Vector3 position)
    {
        transform.localPosition = position;
    }
    public int LayerMask
    {
        get { return layerMask; }
        set { 
            layerMask = value;
            GetComponent<Collider>().gameObject.layer = value;
        }
    }
    private void Awake()
    {
        originalPosition = transform.localPosition;
        touchable = GetComponent<Touchable>();
    }

    public void RestartPosition()
    {
        if (originalPosition != null)
        {
            transform.position = originalPosition;
        }
    }

}
