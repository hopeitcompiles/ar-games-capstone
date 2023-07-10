using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAxis : MonoBehaviour
{
    [SerializeField]
    Button changeModeButton;
    private bool isXmode = false;
    void Start()
    {
        ChangeMode();
        changeModeButton.onClick.AddListener(ChangeMode);
    }

    // Update is called once per frame
    public void ChangeMode()
    {
        isXmode=!isXmode;
        ARinteractionManager.Instance.IsAxisX = isXmode;
        changeModeButton.transform.DORotate(new Vector3(0,180,0),0.5f,RotateMode.Fast);
    }
}
