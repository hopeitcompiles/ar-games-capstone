using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    // Coloca este código en un script adjunto a tu objeto de interés

    void Update()
    {
        // Obtén la posición y tamaño de la caja de colisión del objeto actual
        Vector3 position = transform.position;
        Vector3 size = transform.localScale;

        // Comprueba si hay solapamiento con otros objetos en un radio determinado
        Collider[] colliders = Physics.OverlapBox(position, size / 6f);

        // Itera sobre los colliders encontrados
        foreach (Collider collider in colliders)
        {
            // Verifica si el collider pertenece a otro objeto (excluyendo el objeto actual)
            if (collider.gameObject.TryGetComponent<ContainerPiece>(out ContainerPiece piece))
            {
                if (!piece.IsActive)
                {
                    return;
                }
                piece.IsActive = false;
                piece.gameObject.GetComponent<Touchable>().MakeItGlow(false);
                if (container != null)
                {
                    Vector3 pos = transform.position;
                    pos.y -= 0.5f;
                    var item=Instantiate(piece.gameObject, pos, 
                        Quaternion.identity) as GameObject;
                    item.transform.parent=transform;
                    item.transform.localScale=piece.transform.localScale;
                    //container.Contents.Add(Instantiate(piece.gameObject));
                }
                FillTheContainerARGame.ExplicitInstance.MakePoint(piece.IsCorrect);
                piece.gameObject.SetActive(false);
            }
        }
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
