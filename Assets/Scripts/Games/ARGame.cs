using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARGame : MonoBehaviour
{
    [SerializeField] GameObject modelPrefab;
    protected float[] timeByDificult = { 60, 45, 30 };
    protected float timeLimit;
    protected Camera _camera;

    protected virtual void Start()
    {
        if(modelPrefab == null)
        {
            modelPrefab = (GameObject) Resources.Load("Anatomy");
        }
        _camera = FindAnyObjectByType<Camera>();
        Instantiate(modelPrefab,transform);
        TimerManager.OnRunOutTime += TimerManager_OnRunOutTime;
        timeLimit = timeByDificult[(int)DificultManager.Instance.DificultLevel];
        GameManager.Instance.OnGame += Instance_OnGame;
        GameManager.Instance.OnMainMenu += Instance_OnMainMenu;

    }

    private void Instance_OnMainMenu()
    {
        Debug.Log("Finishing in AR Script");
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void Instance_OnGame()
    {
        StartGame();
    }
    private void TimerManager_OnRunOutTime()
    {
        Debug.Log("start on ARGame class");
        EndGame();
    }

    public abstract void StartGame();
    public abstract void EndGame();
    private void OnDisable()
    {
        TimerManager.Instance.StopTimer();

    }
}
