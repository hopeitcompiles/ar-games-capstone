using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public event Action OnPause;
    public event Action OnResume;
    private bool isPaused;
    private static PauseManager instance;

    public static PauseManager Instance
    {
        get { return Singleton<PauseManager>.Instance;}
    }

    public bool IsPaused
    {
        get { return isPaused; }
    }
    private PauseManager() { }
    void Start()
    {
        isPaused = false;
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        OnPause?.Invoke();
        Debug.Log("pause game");
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        OnResume?.Invoke();
        Debug.Log("resume game");
    }

    
}
