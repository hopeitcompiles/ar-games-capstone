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


    private ARRaycastManager raycastManager;
    private ContainerPiece selectedPiece;


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
        timeByDificult[1] = 3000;
        raycastManager = FindObjectOfType<ARRaycastManager>();
        raycastManager.SetTrackablesActive(true);
        base.Start();
        PauseManager.Instance.OnPause += Instance_OnPause;
        PauseManager.Instance.OnResume += Instance_OnResume;
    }

    private void Instance_OnResume()
    {
        canvas.transform.DOScale(Vector3.one, 0.4f).SetUpdate(true);

    }

    private void Instance_OnPause()
    {
        canvas.transform.DOScale(Vector3.zero, 0.4f).SetUpdate(true);
    }

    public override void StartGame()
    {
        correctPieces=GetComponentsInChildren<ContainerPiece>().ToList();

        for (int i = 0; i < correctPieces.Count; i++)
        {
            correctPieces[i].gameObject.AddComponent<Dragable>();
            correctPieces[i].SetUp(1);

        }
        TimerManager.Instance.StartTimer(timeLimit, false);
        hasStarted = true;
        canvas.SetActive(true);
    }
    
    public override void EndGame()
    {
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
        metric.successCount++;
        metric.score = metric.successCount - metric.failureCount;
    }
}
