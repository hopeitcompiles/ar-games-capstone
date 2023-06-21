using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public enum Result
{
    GOOD,OK,BAD
}
public class ResultsManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private AudioClip audioOk;    
    [SerializeField]
    private AudioClip audioGood;    
    [SerializeField]
    private AudioClip audioBad;

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

    public void Activate(bool state, Result result)
    {
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        parent.DOScale(scale, 0.3f).SetUpdate(true);
        if(!state)
        {
            return;
        }
        if(result==Result.GOOD && audioGood != null)
        {
            AudioManager.Instance.PlayOnShot(audioGood);
            return;
        }
        if (result == Result.BAD && audioBad != null)
        {
            AudioManager.Instance.PlayOnShot(audioBad);
            return;
        }

        if (audioOk!=null)
        {
            AudioManager.Instance.PlayOnShot(audioOk);
        }
    }
    public void Close()
    {
        GameManager.Instance.MainMenu();
    }
}
