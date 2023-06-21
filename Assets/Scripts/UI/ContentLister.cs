using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class ContentLister : MonoBehaviour
{
    public static ContentLister instance;
    [SerializeField] MicroGame[] microGames;
    [SerializeField] AnatomicSystem[] anatomicSystems;
    [SerializeField] GameObject gamesContainer;
    [SerializeField] GameObject systemsContainer; 
    [SerializeField] GameObject uiAvailableModelsContainer;
    [SerializeField] Card uiGameCard;
    [SerializeField] Card uiSystemCard;

    TextMeshProUGUI popUpText;
    Transform uiPopUpPannel;

    void Start()
    {
        uiPopUpPannel=transform.GetChild(2);
        uiPopUpPannel.localScale = Vector3.zero;
        popUpText=uiPopUpPannel.GetComponentInChildren<TextMeshProUGUI>();
        instance = this;
        foreach(MicroGame microGame in microGames) {
            Card newItem;
            newItem = Instantiate(uiGameCard, gamesContainer.transform);
            newItem.Title = microGame.title;
            newItem.Description = microGame.description;
            newItem.Image = microGame.image;
            newItem.Systems = microGame.models;
        }
        foreach(AnatomicSystem system in anatomicSystems) {
            Card newItem;
            newItem = Instantiate(uiGameCard, systemsContainer.transform);
            newItem.Title = system.title;
            newItem.Description = system.description;
            newItem.Image = system.image;
            newItem.Model = system.model;
        }
    }



    public void StartAR(List<MicroGame.SystemData> systems, GameObject model, string title, string description)
    {
        if (model == null && systems==null)
        {
            ShowPopUpPanel();
            popUpText.text = "No hay sistema anatómico configurado para iniciar el modo AR";
            return;
        }
        

        if (model != null)
        {
            Debug.Log("Inicia modo AR");
            ARinteractionManager interactionManager = FindAnyObjectByType<ARinteractionManager>();
            interactionManager.ItemModel = Instantiate(model);
            GameManager.Instance.ARPosition();
            return;
        }

        ShowPopUpPanel();
        if(systems.Count == 0)
        {
            popUpText.text = "No hay sistemas anatómicos configurados para iniciar este juego";
            return;
        }
        foreach (MicroGame.SystemData obj in systems)
        {
            Card newItem;
            newItem = Instantiate(uiSystemCard, uiAvailableModelsContainer.transform);
            newItem.Title = obj.system.title;
            newItem.Description = obj.system.description;
            newItem.Image = obj.system.image;
            newItem.Model = obj.gameObject;
        }
        popUpText.text = description+"\nEscoge el sistema para jugar";
    }


    public void HidePopUpPanel()
    {
        uiPopUpPannel.DOScale(Vector3.zero, 0.3f);
        foreach (Transform child in uiAvailableModelsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void ShowPopUpPanel()
    {
        uiPopUpPannel.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
    }

}
