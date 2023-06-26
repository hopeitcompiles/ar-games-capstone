using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class FindTheImpostorARGame : ARGame
{
    [SerializeField]
    TextMeshProUGUI UIPoints; 
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    Button nextButton;
    [SerializeField]
    Button cancelButton;

    private GameObject selectedPiece;
    private List<ImpostorPiece> pieces;
    int reduceMultiplier=0;
    
    public override void EndGame()
    {
        OnPauseGame();
        float time=TimerManager.Instance.StopTimer();
        Result result;
        if (metric.score > 7)
        {
            result = metric.score >= 9 ? Result.GOOD : Result.OK;
        }
        else
        {
            result = metric.score < 5 ? Result.IMCOMPLETE : Result.BAD;
        }
        metric.timeElapsed = time;
        ResultsManager.Instance.Activate(true,result, metric);
    }

    public override void StartGame()
    {
        if (DificultManager.Instance.DificultLevel != DificultLevel.EASY)
        {
            reduceMultiplier = DificultManager.Instance.DificultLevel == DificultLevel.MEDIUM ? 1 : 2;
        }
        Debug.Log("game has started"); 
        TimerManager.Instance.StartTimer(timeLimit, false);
        canvas.SetActive(true);
        canvas.transform.DOScale(Vector3.one, .3f);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        if(nextButton != null)
        {
            nextButton.transform.localScale = Vector3.zero;
        }
        if(cancelButton != null)
        {
            cancelButton.transform.localScale = Vector3.zero;
        }
        base.Start();
        canvas.transform.localScale = Vector3.zero;
        canvas.SetActive(false);

        pieces=GetComponentsInChildren<ImpostorPiece>().ToList();
        foreach (ImpostorPiece piece in pieces)
        {
            piece.SetUp(0);
        }
        nextButton.onClick.AddListener(NextMove);
        cancelButton.onClick.AddListener(CancelMove);
        PauseManager.Instance.OnPause += OnPauseGame;
        PauseManager.Instance.OnResume += OnResumeGame;
    }
    private void OnPauseGame()
    {
        RectTransform rectTransform = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(new Vector2(0, -rectTransform.rect.height * 1.5f), 0.3f).SetUpdate(true);
    }
    private void OnResumeGame()
    {
        RectTransform rectTransform = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(new Vector2(0, 346.2505f), 0.3f).SetUpdate(true);
    }


    private void MakePoint(bool marked)
    {
        Debug.Log(pieces.Count);
        if(marked)
        {
            metric.successCount++;
        }
        else
        {
            metric.failureCount++;
        }
        AudioManager.Instance.CorrectPlay(marked);

        metric.score = 10 * ((double)metric.successCount / pieces.Count)
        - (reduceMultiplier > 0 ? 5*(double)reduceMultiplier*metric.failureCount/pieces.Count : 0);

        metric.percentageOfCompletion= 100 * ((double)(metric.successCount) / pieces.Count);
        if (UIPoints != null)
        {
            UIPoints.text =metric.score.ToString(metric.score % 2 ==0 ? "0" : "0.00") + (metric.score==1?" punto": " puntos");
        }
    }
    public void CancelMove()
    {
        if(selectedPiece != null)
        {
            if (selectedPiece.TryGetComponent<Touchable>(out var touch))
            {
                touch.MakeItGlow(false);
            }
        }
        FadeButtons(false);
        selectedPiece = null;
    }
    public void NextMove()
    {
        if (selectedPiece==null)
        {
            return;
        }
        if (selectedPiece.TryGetComponent<ImpostorPiece>(out var impostorPiece))
        {
            MakePoint(true);
            impostorPiece.gameObject.SetActive(false);
        }
        else
        {
            MakePoint(false);
            selectedPiece.SetActive(false);
        }
        FadeButtons(false);
        selectedPiece = null;
        if (pieces.Count <= metric.successCount)
        {
            EndGame();
        }

    }
    private void FadeButtons(bool state)
    {
        Vector3 scale=state? Vector3.one : Vector3.zero;
        if (nextButton != null)
        {
            nextButton.transform.DOScale(scale, 0.3f).SetUpdate(true);
        }
        if (cancelButton != null)
        {
            cancelButton.transform.DOScale(scale, 0.3f).SetUpdate(true);
        }
    }
    void FixedUpdate()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        Touch touch = Input.touches[0];
        Vector3 position = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(position);
            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            if (Physics.Raycast(ray, out hit))
            {
                if(selectedPiece != null)
                {
                    selectedPiece.GetComponent<Touchable>().MakeItGlow(false);
                }
                selectedPiece=hit.collider.gameObject;
                if(DificultManager.Instance.DificultLevel==DificultLevel.HARD)
                {
                    NextMove();
                    return;
                }
                FadeButtons(true);
                if (hit.collider.gameObject.TryGetComponent<Touchable>(out var touchable))
                {
                    if (touchable.gameObject.TryGetComponent<Data>(out var data))
                    {
                        UIPoints.text = data.PartName;
                    }
                    touchable.MakeItGlow(true);
                }

            }
        }
    }
}
