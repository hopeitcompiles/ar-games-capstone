using UnityEngine;

[RequireComponent(typeof(Touchable))]
[RequireComponent(typeof(Data))]
public class PuzzlePiece : MonoBehaviour, IPiece
{
    Rigidbody rb;
    Touchable touchable;
    public Touchable Touchable
    {
        get { return touchable; }
    }

    public void MakeItDrop(bool state)
    {
        if (rb == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rb = gameObject.GetComponent<Rigidbody>();
            gameObject.AddComponent<SphereCollider>();
            rb.freezeRotation = true;
            rb.isKinematic = true;

        }
        rb.isKinematic = !state;
    }

    public void SetUp(int layer)
    {
        
    }

    public GameObject GameObject()
    {
        return gameObject;
    }

    public Transform Transform()
    {
        return transform;
    }
}
