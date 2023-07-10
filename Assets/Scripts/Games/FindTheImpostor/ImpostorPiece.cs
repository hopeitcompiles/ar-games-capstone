using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Touchable))]
public class ImpostorPiece : MonoBehaviour, IPiece
{
    private Touchable touchable;

    public Touchable Touchable { get { return touchable; } }

    public GameObject GameObject()
    {
        return gameObject;
    }

    public void SetUp(int layer)
    {
        //gameObject.GetComponent<MeshCollider>().gameObject.layer = layer;
        touchable = gameObject.GetComponent<Touchable>();
    }

    public Transform Transform()
    {
        return transform;
    }
}
