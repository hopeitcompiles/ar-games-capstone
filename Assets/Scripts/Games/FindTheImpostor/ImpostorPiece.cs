using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Touchable))]
[RequireComponent(typeof(Data))]
public class ImpostorPiece : MonoBehaviour, IPiece
{
    [SerializeField]
    private DificultLevel appersFrom=DificultLevel.EASY;

    private Touchable touchable;

    public Touchable Touchable { get { return touchable; } }
    public DificultLevel AppersFrom { get {  return appersFrom; } }
    public GameObject GameObject()
    {
        return gameObject;
    }

    public void SetUp(int layer)
    {
        if (GetComponent<Data>().System != SystemManager.instance.ActiveSystem &&
            (int)appersFrom-1 >= (int)DificultManager.Instance.DificultLevel)
        {
            gameObject.SetActive(false);
        }
        touchable = gameObject.GetComponent<Touchable>();
    }

    public Transform Transform()
    {
        return transform;
    }
}
