using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPiece
{
    public void SetUp(int layer);
    public GameObject GameObject();
    public Transform Transform();
}
