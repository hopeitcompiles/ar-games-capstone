using System.Collections;
using System.Collections.Generic;
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
        active = false;
        GameManager.Instance.OnGame += SetUp;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!active)
        {
            return;
        }
        if (collision.collider.gameObject.TryGetComponent<ContainerPiece>(out var piece))
        {
            if(container != null)
            {
                container.Contents.Add(Instantiate(piece.gameObject));
            }
            piece.gameObject.SetActive(false);
        }
    }
    private void SetUp()
    {
        active = true;
    }
}
