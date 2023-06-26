using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public event Action OnPause;
    public event Action OnResume;
    private bool isPaused;

    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI percentage;
    [SerializeField]
    private TextMeshProUGUI asserts;
    [SerializeField]
    private TextMeshProUGUI fails;

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
        Models.GameMetric stats= ARGame.instance.getStats();
        score.text = stats.score.ToString(stats.score % 1 == 0 ? "0" : "0.00") + (stats.score == 1 ? " punto" : " puntos");
        percentage.text = stats.percentageOfCompletion.ToString(stats.percentageOfCompletion % 1 == 0 ? "0" : "0.00") + "% completado";
        asserts.text = stats.successCount.ToString("0") + (stats.successCount == 1 ? " acierto" : " aciertos");
        fails.text = stats.failureCount.ToString("0") + (stats.failureCount==1? " error":" errores");

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
