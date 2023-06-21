using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARGame : MonoBehaviour
{
    [SerializeField] int gameId;
    [SerializeField] GameObject modelPrefab;
    protected float[] timeByDificult = { 60, 45, 30 };
    protected float timeLimit;
    protected Camera _camera;

    int classId;
    public int ClassId
    {
        get { return classId; }
        set { classId = value; }    
    }

    protected float Score;
    protected float TimeElapsed;
    protected float PercentageOfCompletion;
    protected bool IsGameCompleted;
    protected int SuccessCount;
    protected int FailureCount;
    protected string Difficulty;
    protected string Comments;
    
    protected bool SendGameStats()
    {
        return true;
    }
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
        EndGame();
    }

    public abstract void StartGame();
    public abstract void EndGame();
    private void OnDisable()
    {
        TimerManager.Instance.StopTimer();

    }
}
