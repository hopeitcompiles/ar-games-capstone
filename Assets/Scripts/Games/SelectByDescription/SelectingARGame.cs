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
    GameObject results;

    SelectablePiece[] pieceList;
    string[] responses; 
    int index;
    TextMeshProUGUI title;
    TextMeshProUGUI description;
    SelectablePiece selectedPiece;


    private void Awake()
    {
        canvas=transform.GetChild(0).gameObject;
        results= canvas.transform.GetChild(1).gameObject;
        content.SetActive(false);
        canvas.SetActive(false);
        
        
        GameManager.Instance.OnMainMenu += Instance_OnMainMenu;
        PauseManager.Instance.OnPause += OnPauseGame;
        PauseManager.Instance.OnResume += OnResumeGame;
        nextButton.transform.localScale = Vector3.zero;
        clearButton.transform.localScale = Vector3.zero;
        results.transform.localScale = Vector3.zero;
    }
    protected override void Start()
    {
        base.Start();
        pieceList = GetComponentsInChildren<SelectablePiece>();
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
        results.transform.DOScale(Vector3.zero, 0.3f).SetUpdate(true);

    }    
    private void OnResumeGame()
    {
        content.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
        results.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
    }

    
    public override void StartGame()
    {
        Debug.Log("the game has started");
        title.text = "Toca la parte del sistema digestivo a la cual pertenezca esta descripcion";
        description.text = pieceList[index].Description;
        canvas.SetActive(true);
        content.SetActive(true);
        TimerManager.Instance.StartTimer(timeLimit, false);
    }
    public override void EndGame()
    {
        int asserts = 0;
        for (int i = 0; i < pieceList.Length; i++)
        {
            if (pieceList[i].Description == responses[i])
            {
                asserts++;
            }
        }
        content.transform.DOScale(Vector3.zero, .3f);
        
        results.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =asserts+" aciertos de "+pieceList.Length;
        results.transform.DOScale(Vector3.one, .3f);
    }
    public void NextPiece()
    {
        if (index == 0)
        {
            TimerManager.Instance.StopTimer();
            EndGame();
            return;
        }
        if(selectedPiece!= null)
        {
            responses[index] = selectedPiece.Description;
            index--;
            description.text = pieceList[index].Description;
            ResetTitleAndButtons(true);
            selectedPiece.gameObject.SetActive(false);
            selectedPiece.Touchable.MakeItGlow(false);
        }
        else
        {
            title.text = "Algo no ha salido bien!";
        }
        
    }

    public void ResetTitleAndButtons(bool state)
    {
        Vector3 scale=Vector3.one;
        if (state)
        {
            title.text = "Toca la parte del sistema digestivo a la cual pertenezca esta descripci�n";
            scale = Vector3.zero;
        }
        nextButton.transform.DOScale(scale, .3f);
        clearButton.transform.DOScale(scale, .3f);
    }
    public void CloseGame()
    {
        results.transform.DOScale(Vector3.zero, .3f);
        GameManager.Instance.MainMenu();
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
            if(Physics.Raycast(ray, out hit))
            {
                if(selectedPiece != null)
                {
                    selectedPiece.Touchable.MakeItGlow(false);
                }
                selectedPiece =hit.collider.gameObject.GetComponent<SelectablePiece>();
                if(selectedPiece != null)
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
