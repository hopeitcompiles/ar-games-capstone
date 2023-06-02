using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ResultsManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private AudioClip audioClip;
    private Transform parent;

    private static ResultsManager instance;
    public static ResultsManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
        parent = title.gameObject.transform.parent.parent.parent;
        parent.localScale = Vector3.zero;
        GameManager.Instance.OnMainMenu += Reset;
        GameManager.Instance.OnARPosition += Reset;

    }

    private void Reset()
    {
        title.text = "Resultados";
        description.text = "";
        parent.localScale = Vector3.zero;
    }

    public string Title
    {
        set { title.text = value; }
    }

    public string Description
    {
        set { description.text = value; }
    }

    public void Activate(bool state)
    {
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        parent.DOScale(scale, 0.3f).SetUpdate(true);
        if(state && audioClip!=null)
        {
            AudioManager.Instance.PlayOnShot(audioClip);
        }
    }
    public void Close()
    {
        GameManager.Instance.MainMenu();
    }
}
