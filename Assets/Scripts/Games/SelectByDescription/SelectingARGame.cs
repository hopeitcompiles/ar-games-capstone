using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectingARGame : ARGame
{
    [SerializeField] GameObject content;
    [SerializeField] Button nextButton;
    [SerializeField] Button clearButton;
    GameObject canvas;

    List<SelectablePiece> pieceList;
    SelectablePiece current;
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI description;
    SelectablePiece selectedPiece;
    readonly int layer =6;
    int pieceQuanty;
    private void Awake()
    {
        canvas=transform.GetChild(0).gameObject;
        content.SetActive(false);
        canvas.SetActive(false);
        
        
        GameManager.Instance.OnMainMenu += Instance_OnMainMenu;
        PauseManager.Instance.OnPause += OnPauseGame;
        PauseManager.Instance.OnResume += OnResumeGame;
        nextButton.transform.localScale = Vector3.zero;
        clearButton.transform.localScale = Vector3.zero;
    }
    protected override void Start()
    {
        timeByDificult[2] = 3;
        instance = this;
        base.Start();
        pieceList = GetComponentsInChildren<SelectablePiece>().ToList();
        pieceQuanty = pieceList.Count;
        foreach (SelectablePiece piece in pieceList)
        {
            piece.SetUp(layer);
        }
        nextButton.onClick.AddListener(NextPiece);
        clearButton.onClick.AddListener(ClearSelection);
        
    }
    public void ClearSelection()
    {
        if(selectedPiece != null)
        {
            selectedPiece.Touchable.MakeItGlow(false);
        }
        selectedPiece=null;
        ResetTitleAndButtons(true);
    }

    private void Instance_OnMainMenu()
    {
        canvas.SetActive(false);
        content.SetActive(false);
    }

    

    private void OnPauseGame()
    {
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(new Vector2(0, -rectTransform.rect.height*1.5f), 0.3f).SetUpdate(true);
    }
    private void OnResumeGame()
    {
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(new Vector2(0, 588.1556f), 0.3f).SetUpdate(true);
    }

    
    public override void StartGame()
    {
        LoadNextQuestion();
        Debug.Log("the game has started");
        canvas.SetActive(true);
        content.SetActive(true);
        TimerManager.Instance.StartTimer(timeLimit, false);
    }
    public override void EndGame()
    {
        float time = TimerManager.Instance.StopTimer();
       
        content.transform.DOScale(Vector3.zero, .3f);

        Result result;
        if (metric.score > 7)
        {
            result = metric.score >= 9?Result.GOOD: Result.OK;
        }
        else
        {
            result = metric.score < 5 ? Result.IMCOMPLETE : Result.BAD;
        }
        canvas.SetActive(false);
        metric.timeElapsed = time;
        ResultsManager.Instance.Activate(true, result,metric);
    }
    private void SetDefaultTitle()
    {
        title.text = "Toca la parte a la que pertenece esta descripción";

    }
    public void NextPiece()
    {
        bool isCorrect = false;
        if(selectedPiece!= null)
        {
            if (selectedPiece.PieceName == current.PieceName)
            {
                isCorrect = true;
                metric.successCount++;
                metric.score = 10*((double)metric.successCount /pieceQuanty);
            }
            else{
                metric.failureCount++;
            }
            metric.percentageOfCompletion = 100 * ((double)(metric.successCount + metric.failureCount) / pieceQuanty);
            if (pieceList.Count >= 1)
            {
                AudioManager.Instance.CorrectPlay(isCorrect);
                if (isCorrect)
                {
                    Vibration.instance.Vibrate();
                    if (DificultManager.Instance.DificultLevel != DificultLevel.EASY)
                    {
                        selectedPiece.gameObject.SetActive(false);
                    }
                }
                LoadNextQuestion();
                ResetTitleAndButtons(true);
                selectedPiece.Touchable.MakeItGlow(false);
            }
        }
        else
        {
            SetDefaultTitle();
        }
        if (metric.failureCount+metric.successCount == pieceQuanty)
        {
            EndGame();
            return;
        }
    }
    private void LoadNextQuestion()
    {
        if(pieceList.Count <= 0)
        {
            return;
        }
        int index =pieceList.Count==1?0: Random.Range(0, pieceList.Count);
        current = pieceList[index];
        pieceList.RemoveAt(index);
        description.text = current.Description;
        SetDefaultTitle();
    }
    public void ResetTitleAndButtons(bool state)
    {
        Vector3 scale=Vector3.one;
        if (state)
        {
            SetDefaultTitle();
            scale = Vector3.zero;
        }
        nextButton.transform.DOScale(scale, .3f);
        clearButton.transform.DOScale(scale, .3f);
    }

    void FixedUpdate()
    {
        if(Input.touchCount == 0)
        {
            return;
        }
        Touch touch = Input.touches[0];
        Vector3 position = touch.position;

        if(touch.phase==TouchPhase.Began )
        {
            RaycastHit hit;
            Ray ray=_camera.ScreenPointToRay(position);
            //if(Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            if(Physics.Raycast(ray, out hit))
            {
                if(selectedPiece != null)
                {
                    selectedPiece.Touchable.MakeItGlow(false);
                }
                if(hit.collider.gameObject.TryGetComponent(out selectedPiece))
                {
                    if (DificultManager.Instance.DificultLevel == DificultLevel.HARD)
                    {
                        NextPiece();
                        return;
                    }
                    selectedPiece.Touchable.MakeItGlow(true);
                    title.text = selectedPiece.PieceName;
                    nextButton.transform.DOScale(Vector3.one, .3f);
                    clearButton.transform.DOScale(Vector3.one, .3f);
                }
            } 
        }
    }
}
