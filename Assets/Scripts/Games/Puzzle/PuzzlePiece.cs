using UnityEngine;

[RequireComponent(typeof(Touchable))]
[RequireComponent(typeof(Data))]
public class PuzzlePiece : MonoBehaviour, IPiece
{
    [SerializeField]
    bool isOriginal;
    bool isPlaced;

    Rigidbody rb;
    Touchable touchable;
    public bool IsPlaced
    {
        get { return isPlaced; }
        set { isPlaced = value; }
    }

    public bool IsOriginal
    {
        get { return isOriginal; }
    }

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
        isPlaced = true;
    }

    public GameObject GameObject()
    {
        return gameObject;
    }

    public Transform Transform()
    {
        return transform;
    }

    private void Update()
    {
        if (isOriginal)
        {
            Vector3 position = transform.position;
            Vector3 size = transform.localScale;

            // Comprueba si hay solapamiento con otros objetos en un radio determinado
            Collider[] colliders = Physics.OverlapBox(position, size / 6f);

            // Itera sobre los colliders encontrados
            foreach (Collider collider in colliders)
            {
                // Verifica si el collider pertenece a otro objeto (excluyendo el objeto actual)
                if (collider.gameObject.TryGetComponent<PuzzlePiece>(out PuzzlePiece piece))
                {
                    if (piece.IsOriginal)
                    {
                        return;
                    }
                    piece.transform.position = transform.position;
                }
            }
        }
    }
}
