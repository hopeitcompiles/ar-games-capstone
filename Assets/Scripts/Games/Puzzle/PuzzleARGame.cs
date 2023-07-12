using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleARGame : ARGame
{
    [SerializeField]
    Button finishButton;
    [SerializeField]
    GameObject infoIcon;
    [SerializeField]
    GameObject confirmPanel;

    [SerializeField]
    GameObject canvas;
    [SerializeField]
    TextMeshProUGUI title;
    PuzzlePiece[] pieces;
    List<PuzzlePiece> target;
    readonly float proximityThreshold = 0.08f;

    PuzzlePiece selectedPiece;
    PuzzlePiece lastSelectedPiece;
    int pieceCount; 
    int reduceMultiplier = 0;

    Dictionary<string, string> responses;
    public override void EndGame()
    {
        int empty = 0;
        foreach (var response in responses)
        {
            if (response.Value == "")
            {
                empty++;
            }
            else
            {
                if (response.Key == response.Value)
                {
                    metric.successCount++;
                }
                else
                {
                    metric.failureCount++;
                }
            }
            
        }
        metric.score = 10 * ((double)metric.successCount / pieceCount)
        - (reduceMultiplier > 0 ? 5 * (double)reduceMultiplier * metric.failureCount / pieceCount : 0);
        if(metric.score<0)
        {
            metric.score = 0;
        }
        metric.percentageOfCompletion = 100 * ((double)(metric.successCount) / pieceCount);

        canvas.SetActive(false);
        metric.isGameCompleted = false;
        metric.timeElapsed = TimerManager.Instance.StopTimer();
        ResultsManager.Instance.Activate(true, Result.OK, metric);

    }
    public void ConfirmFinish()
    {
        Time.timeScale = 0;
        confirmPanel.SetActive(true);
        confirmPanel.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
    }
    public void CancelFinish()
    {
        Time.timeScale = 1;
        confirmPanel.transform.DOScale(Vector3.zero, 0.3f).SetUpdate(true);
    }
    public override void StartGame()
    {
        if (DificultManager.Instance.DificultLevel != DificultLevel.EASY)
        {
            reduceMultiplier = DificultManager.Instance.DificultLevel == DificultLevel.MEDIUM ? 1 : 2;
        }
        Debug.Log("Started Puzzle Game");
        canvas.SetActive(true);
        TimerManager.Instance.StartTimer(timeLimit, false);
        finishButton.onClick.AddListener(ConfirmFinish);
    }

    protected override void Start()
    {
        canvas.SetActive(false);
        confirmPanel.SetActive(false );
        finishButton.transform.localScale= Vector3.zero;
        finishButton.gameObject.SetActive(false);
        base.Start();
        pieceCount = 0;
        target = new();
        responses = new();
        pieces = GetComponentsInChildren<PuzzlePiece>();
        foreach (PuzzlePiece piece in pieces)
        {
            if (piece.IsOriginal)
            {
                pieceCount++;
                target.Add(piece);
                responses[piece.GetComponent<Data>().PartName] = "";
            }
            else
            {
                piece.SetUp(1);
                piece.gameObject.AddComponent<Dragable>();
            }
        }
        PauseManager.Instance.OnPause += Instance_OnPause;
        PauseManager.Instance.OnResume += Instance_OnResume; ;
    }

    private void Instance_OnResume()
    {
        canvas.SetActive(true);
    }

    private void Instance_OnPause()
    {
        canvas.SetActive(false);
    }

    private void Update()
    {
        selectedPiece=(PuzzlePiece)ARinteractionManager.Instance.Manage3DModelDrag<PuzzlePiece>();
        if(selectedPiece != null)
        {
            if (!finishButton.gameObject.activeSelf)
            {
                finishButton.gameObject.SetActive(true);
                finishButton.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
            }
            lastSelectedPiece = selectedPiece;
        }
        else if(lastSelectedPiece != null) {
            foreach (PuzzlePiece targetObject in target)
            {
                Vector3 moveObjectPosition = lastSelectedPiece.transform.position;
                // Posición del objeto objetivo
                Vector3 targetObjectPosition = targetObject.transform.position;

                // Calcula la distancia entre los dos objetos
                float distance = Vector3.Distance(moveObjectPosition, targetObjectPosition);

                // Comprueba si están lo suficientemente cerca
                if (distance < proximityThreshold && !lastSelectedPiece.IsPlaced)
                {
                    // Toma la posición del objeto objetivo
                    lastSelectedPiece.transform.position = targetObjectPosition;
                    lastSelectedPiece.IsPlaced = true;
                    responses[targetObject.GetComponent<Data>().PartName] =
                        targetObject.GetComponent<Data>().PartName;
                    break; // Sale del bucle si se encontró un objeto cercano
                }
            }
            lastSelectedPiece.IsPlaced = false;
        }

    } 
}
