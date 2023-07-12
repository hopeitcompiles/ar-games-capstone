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
        //changeModeButton.onClick.AddListener(ChangeMode);
    }

    // Update is called once per frame
    public void ChangeMode()
    {
        isXmode=!isXmode;
        ARinteractionManager.Instance.IsAxisX = isXmode;
        changeModeButton.GetComponent<RectTransform>().transform.DORotate(new Vector3(0, 0, (isXmode ? 0 : 90)),0.3f,RotateMode.FastBeyond360);
            //Quaternion.Euler(0f, 0f, 90f) :
            //Quaternion.Euler(0f, 0f, 0f));
    }
}
