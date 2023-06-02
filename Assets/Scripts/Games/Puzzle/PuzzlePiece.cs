using Lean.Touch;
using UnityEngine;

[RequireComponent(typeof(Touchable))]
[RequireComponent(typeof(Data))]
public class PuzzlePiece : MonoBehaviour, IPiece
{
    Rigidbody rb;
    Touchable touchable;
    LeanDragTranslate lean;
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
        gameObject.GetComponent<MeshCollider>().gameObject.layer = layer;
        touchable = gameObject.GetComponent<Touchable>();
        if (lean == null)
        {
            if (!gameObject.TryGetComponent(out lean))
            {
                lean = gameObject.AddComponent<LeanDragTranslate>();
            }
            lean.Camera = FindAnyObjectByType<Camera>();
        }
    }
}
