using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Data))]
[RequireComponent(typeof(Touchable))]
public class SelectablePiece : MonoBehaviour, IPiece
{
    private Data data;
    private Touchable touchable;

    public Touchable Touchable { get { return touchable; } }
    public string PieceName
    {
        get { return data.PartName; }
    }
    public string Description
    {
        get { return data.GetRamdonFact(DificultManager.Instance.DificultLevel); }
    }

    public GameObject GameObject()
    {
        return gameObject;
    }

    public void SetUp(int layer)
    {
        gameObject.GetComponent<Collider>().gameObject.layer = layer;
        data = gameObject.GetComponent<Data>();
        touchable = gameObject.GetComponent<Touchable>();
    }

    public Transform Transform()
    {
        return transform;
    }
}
