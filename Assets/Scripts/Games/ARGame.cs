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
    protected  Models.GameMetric metric;
    protected bool hasStarted;
    public static ARGame instance;

    protected GameObject Model
    {
        get { return modelPrefab; }
    }
    public Models.GameMetric getStats()
    {
        return metric;
    }

    protected bool SendGameStats()
    {
        return true;
    }
    protected virtual void Start()
    {
        instance = this;
        metric=new Models.GameMetric();
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
        metric.difficulty = DificultManager.Instance.DificultLevel.ToString();
        metric.gameId = gameId;
        metric.userId = Profile.instance.User != null ? Profile.instance.User.id : -1 ;
        hasStarted = false;
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
