using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uIMainMenuPanel;
    [SerializeField]
    private GameObject uIARPlacingPanel;
    [SerializeField]
    private GameObject uIARGamePanel;


    void Start()
    {
        GameManager.Instance.OnMainMenu += Instance_OnMainMenu;
        GameManager.Instance.OnARPosition += Instance_OnARPosition;
        GameManager.Instance.OnGame += Instance_OnGame;
        PauseManager.Instance.OnPause += UIManager_OnPause;
        PauseManager.Instance.OnResume += UIManager_OnResume;
        uIARGamePanel.transform.localScale = Vector3.zero;
        uIARPlacingPanel.transform.localScale = Vector3.zero;
    }

    private void UIManager_OnResume()
    {
        FadeARPauseMenu(false);
        Time.timeScale = 1.0f;
    }

    private void UIManager_OnPause()
    {
        FadeARPauseMenu(true);
        Time.timeScale = 0f;
    }

    private void Instance_OnGame()
    {
        FadeMenu(false);
        FadeARGame(true);
        FadeARPlacing(false);
    }

    private void Instance_OnMainMenu()
    {
        FadeMenu(true);
        FadeARPlacing(false);
        FadeARGame(false);
        UIManager_OnResume();
    }

    private void Instance_OnARPosition()
    {
        FadeMenu(false);
        FadeARPlacing(true);
    }

    private void FadeMenu(bool state)
    {
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        uIMainMenuPanel.transform.DOScale(scale, 0.3f);
    }
    private void FadeARPlacing(bool state)
    {
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        uIARPlacingPanel.transform.DOScale(scale, 0.3f);
    }
    private void FadeARGame(bool state)
    {
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        uIARGamePanel?.transform.DOScale(scale, 0.3f);
    }
    private void FadeARPauseMenu(bool state)
    {
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        Vector3 scale2 = !state ? Vector3.one : Vector3.zero;
        uIARGamePanel?.transform.GetChild(1).DOScale(scale, 0.3f).SetUpdate(true);
        uIARGamePanel?.transform.GetChild(0).DOScale(scale2, 0.3f).SetUpdate(true);

    }
}
