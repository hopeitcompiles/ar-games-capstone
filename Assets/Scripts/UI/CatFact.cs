using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatFact : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI catText;
    [SerializeField]
    Button reload;

    private void Start()
    {
        if (reload==null)
        {
            reload=GetComponentInChildren<Button>();
        }
        reload.transform.localScale = Vector3.zero;
    }

    public async void GetCatFact()
    {
        reload.transform.localScale = Vector3.zero;
        ServiceApi service = new();
        string response=await service.GetCatFact();
        reload.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
        catText.text = response.TrimStart();
    }
}
