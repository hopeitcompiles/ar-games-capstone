using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatFact : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI catText;
    void Start()
    {
        GetCatFact();
    }

    public async void GetCatFact()
    {
        ServiceApi service = new();
        ServiceApi.FactCat response=await service.GetCatFact();
        catText.text = response.fact;
    }
}
