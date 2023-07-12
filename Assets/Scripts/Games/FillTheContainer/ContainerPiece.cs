using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(Touchable))]
public class ContainerPiece : MonoBehaviour, IPiece
{
    [SerializeField]
    bool isNotCorrect;
    bool isActive;
    public bool IsCorrect { get {  return !isNotCorrect; } }
    public bool IsActive { get { return isActive; }set { isActive = value; } }
    public GameObject GameObject()
    {
        return gameObject;
    }

    public void SetUp(int layer)
    {
        isActive = true;
        GetComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;
        Rigidbody rigidbody= GetComponent<Rigidbody>();  
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
        }
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
