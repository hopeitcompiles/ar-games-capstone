using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectingARGame : ARGame
{
    [SerializeField] GameObject content;
    [SerializeField] Button nextButton;
    [SerializeField] Button clearButton;
    GameObject canvas;

    SelectablePiece[] pieceList;
    string[] responses; 
    int index;
    TextMeshProUGUI title;
    TextMeshProUGUI description;
    SelectablePiece selectedPiece;
    readonly int layer =6;

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
        base.Start();
        pieceList = GetComponentsInChildren<SelectablePiece>();
        foreach (SelectablePiece piece in pieceList)
        {
            piece.SetUp(layer);
        }
        index = pieceList.Length - 1;
        title = content.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        description = content.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        responses = new string[pieceList.Length];


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
        content.transform.DOScale(Vector3.zero, 0.3f).SetUpdate(true);

    }    
    private void OnResumeGame()
    {
        content.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
    }

    
    public override void StartGame()
    {
        Debug.Log("the game has started");
        title.text = "Toca la parte del sistema a la cual pertenece esta descripción";
        description.text = pieceList[index].Description;
        canvas.SetActive(true);
        content.SetActive(true);
        TimerManager.Instance.StartTimer(timeLimit, false);
    }
    public override void EndGame()
    {
        float time = TimerManager.Instance.StopTimer();
        int asserts = 0;
        for (int i = 0; i < pieceList.Length; i++)
        {
            if (pieceList[i].PieceName == responses[i])
            {
                asserts++;
            }
        }
        content.transform.DOScale(Vector3.zero, .3f);
        
        ResultsManager.Instance.Description=asserts+" aciertos de "+pieceList.Length+ "\nTiempo: " + time.ToString() + " segundos";
        ResultsManager.Instance.Activate(true);
    }
    public void NextPiece()
    {
        if (index == 0)
        {
            EndGame();
            return;
        }
        if(selectedPiece!= null)
        {
            responses[index] = selectedPiece.PieceName;
            index--;
            description.text = pieceList[index].Description;
            ResetTitleAndButtons(true);
            selectedPiece.gameObject.SetActive(false);
            selectedPiece.Touchable.MakeItGlow(false);
        }
        else
        {
            title.text = "Toca la parte del sistema a la cual pertenece esta descripción";
        }
        
    }

    public void ResetTitleAndButtons(bool state)
    {
        Vector3 scale=Vector3.one;
        if (state)
        {
            title.text = "Toca la parte del sistema a la cual pertenece esta descripción";
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
