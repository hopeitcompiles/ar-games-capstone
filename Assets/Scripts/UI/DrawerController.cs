using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerController : MonoBehaviour
{
    [SerializeField]
    private GameObject drawerPanel;
    public static DrawerController instance;
    readonly float animationTime = 0.5f;
    void Awake()
    {
        instance = this;
        drawerPanel.transform.localScale = Vector3.zero;
        drawerPanel.SetActive(true);
        CloseDrawer();
    }
    public void ShowDrawer()
    {
        HandleDrawer(true);
    }
    public void CloseDrawer()
    {
        HandleDrawer(false);
    }

       
    private void HandleDrawer(bool state)
    {
        GameObject background = drawerPanel.transform.GetChild(0).gameObject;
        RectTransform rectTransform = drawerPanel.GetComponent<RectTransform>();
        if (state)
        {
            drawerPanel.SetActive(true);
            drawerPanel.transform.localScale = Vector3.one;
            rectTransform.transform.localPosition = new Vector3(-Screen.width * 0.5f, 0, 0);
            rectTransform.DOAnchorPos(Vector2.zero, animationTime, false).SetEase(Ease.InOutQuint);
            background.GetComponent<Image>().DOFade(0.77f, animationTime * 0.8f);
            return;
        }
        rectTransform.transform.localPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(-Screen.width - rectTransform.rect.width / 2, 0), animationTime, false)
            .SetEase(Ease.InOutQuart);
        background.GetComponent<Image>().DOFade(0, animationTime);
    }
}

