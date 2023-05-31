using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Touchable))]
public class ImpostorPiece : MonoBehaviour
{
    private int layerMask;
    private Touchable touchable;

    public Touchable Touchable { get { return touchable; } }
    
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
        touchable = GetComponent<Touchable>();
    }

}
