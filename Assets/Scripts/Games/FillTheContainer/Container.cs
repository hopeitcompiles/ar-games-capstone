using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private bauScript container;
    private bool active;

    private void Start()
    {
        if (container == null)
        {
            container = gameObject.GetComponentInParent<bauScript>();
        }
        active = true;
        GameManager.Instance.OnGame += SetUp;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collision");
    //    if (!active)
    //    {
    //        return;
    //    }
    //    if (collision.collider.gameObject.TryGetComponent<ContainerPiece>(out var piece))
    //    {
    //        if(container != null)
    //        {
    //            container.Contents.Add(Instantiate(piece.gameObject));
    //        }
    //        FillTheContainerARGame.ExplicitInstance.MakePoint(true);
    //        piece.gameObject.SetActive(false);
    //    }
    //}
    private void SetUp()
    {
        active = true;
    }
}
