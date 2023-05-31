using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickeablePiece : MonoBehaviour
{
    [SerializeField] string description;
    public void MakeActive()
    {
        Debug.Log(name + " is active");
    }
}
