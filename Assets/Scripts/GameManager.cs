using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public event Action OnMainMenu;
    public event Action OnARPosition;
    public event Action OnGame;
    public event Action OnProfile;

    public static GameManager Instance
    {
        get
        {
            return Singleton<GameManager>.Instance;
        }
    }

    void Start()
    {
        MainMenu();
    }
    public void MainMenu()
    {
        OnMainMenu?.Invoke();
        Debug.Log("Main menu");
    }

    public void ARPosition()
    {
        OnARPosition?.Invoke();
        Debug.Log("ARPosition");
    }
  
    public void ARGame()
    {
        OnGame?.Invoke();
        Debug.Log("Gaming");
    }
    public void Profile()
    {
        OnProfile?.Invoke();
        Debug.Log("Profile");
    }


    public void CloseApp()
    {
        Debug.Log("Close App");
        Application.Quit();
    }
}
