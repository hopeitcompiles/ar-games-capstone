using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Data))]
[RequireComponent(typeof(Touchable))]
public class SelectablePiece : MonoBehaviour
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

    private void Start()
    {
        data = GetComponent<Data>();
        touchable = GetComponent<Touchable>();
    }
  
}
