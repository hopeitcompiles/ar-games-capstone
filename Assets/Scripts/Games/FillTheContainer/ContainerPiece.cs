using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(Touchable))]
public class ContainerPiece : MonoBehaviour, IPiece
{
    public GameObject GameObject()
    {
        return gameObject;
    }

    public void SetUp(int layer)
    {
        GetComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;
        Rigidbody rigidbody=gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public Transform Transform()
    {
       return transform;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("from container piece");
    }




}
