using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class FillTheContainerARGame : ARGame
{
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    TextMeshProUGUI title;
    private List<ContainerPiece> correctPieces=new();
    private int correctCount;

    private ARRaycastManager raycastManager;
    private ContainerPiece selectedPiece;
    int reduceMultiplier = 0;

    public static FillTheContainerARGame ExplicitInstance;
    private string lastPieceSelected;
    private void Awake()
    {
        ExplicitInstance = this;
    }
    protected override void Start()
    {
        if (canvas == null)
        {
            canvas = transform.GetChild(0).gameObject;
        }
        canvas.SetActive(false);
        raycastManager = FindObjectOfType<ARRaycastManager>();
        raycastManager.SetTrackablesActive(true);
        base.Start();
    }

    public override void OnResumeGame()
    {
        //canvas.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true);
        canvas.SetActive(true);
    }

    public override void OnPauseGame()
    {
        canvas.SetActive(false);
        //canvas.transform.DOScale(Vector3.zero, 0.4f).SetUpdate(true);
    }

    public override void StartGame()
    {
        correctPieces=GetComponentsInChildren<ContainerPiece>().ToList();
        if (DificultManager.Instance.DificultLevel != DificultLevel.EASY)
        {
            reduceMultiplier = DificultManager.Instance.DificultLevel == DificultLevel.MEDIUM ? 1 : 2;
        }
        correctCount = 0;
        foreach (ContainerPiece piece in correctPieces)
        {
            piece.gameObject.AddComponent<Dragable>();
            piece.SetUp(1);
            if (piece.IsCorrect)
            {
                correctCount++;
            }

        }
        Debug.Log(correctCount.ToString());
        Debug.Log(correctPieces.Count.ToString());
        TimerManager.Instance.StartTimer(timeLimit, false);
        hasStarted = true;
        canvas.SetActive(true);
    }
    
    public override void EndGame()
    {
        canvas.SetActive(false);
        metric.timeElapsed = TimerManager.Instance.StopTimer();
        ResultsManager.Instance.Activate(true, Result.OK, metric);
    }
    private void Update()
    {
        if (!hasStarted)
        {
            return;
        }
        selectedPiece=(ContainerPiece)ARinteractionManager.Instance.Manage3DModelDrag<ContainerPiece>();
        
        if(selectedPiece != null)
        {
            lastPieceSelected= selectedPiece.GetComponent<Data>().PartName;
        }
        else
        {
            lastPieceSelected = "Toca con tu dedo una pieza y muévela";
        }
        if(title.text!=lastPieceSelected)
        {
            title.text = lastPieceSelected;
        }
    }

    public void MakePoint(bool correct)
    {
        if (correct)
        {
            metric.successCount++;
            
        }
        else
        {
            metric.failureCount++;
        }
        if (metric.successCount < correctCount)
        {
            AudioManager.Instance.CorrectPlay(correct);
        }
        metric.score = 10 * ((double)metric.successCount / correctCount)
       - (reduceMultiplier > 0 ? 5 * (double)reduceMultiplier * metric.failureCount / correctCount : 0);

        metric.percentageOfCompletion = 100 * ((double)(metric.successCount) / correctCount);
        if(metric.successCount >= correctCount) {
            EndGame();
        }
    }
}
