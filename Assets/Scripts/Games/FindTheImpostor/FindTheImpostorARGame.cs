using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class FindTheImpostorARGame : ARGame
{
    [SerializeField]
    TextMeshProUGUI UIPoints; 
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject nextButton;
    [SerializeField]
    GameObject cancelButton;
    [SerializeField]
    GameObject impostorsModel;
    [SerializeField]
    private AudioClip correctAudio;
    [SerializeField]
    private AudioClip failAudio;
    private GameObject selectedPiece;
    private ImpostorPiece[] pieces;
    int asserts = 0;
    int fails = 0;

    int layer=6;
    public override void EndGame()
    {
        float time=TimerManager.Instance.StopTimer();
        ResultsManager.Instance.Title = "Resultados";
        ResultsManager.Instance.Description = "Aciertos: " + asserts.ToString()+"\nErrores: "+fails.ToString()+ "\nTiempo: "+time.ToString()+" segundos";
        Result result;
        if (asserts - fails > pieces.Length)
        {
            result = Result.GOOD;
        }
        else
        {
            result = asserts < fails ? Result.BAD : Result.OK;
        }
        ResultsManager.Instance.Activate(true,result);
    }

    public override void StartGame()
    {
        Debug.Log("game has started"); 
        TimerManager.Instance.StartTimer(timeLimit, false);
        canvas.SetActive(true);
        canvas.transform.DOScale(Vector3.one, .3f);
        asserts = 0;
        fails = 0;
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
        pieces=GetComponentsInChildren<ImpostorPiece>();
        foreach (ImpostorPiece piece in pieces)
        {
            piece.SetUp(layer);
        }
    }
    private void MakePoint(bool marked)
    {
        if(marked)
        {
            asserts++;
            AudioManager.Instance.PlayOnShot(correctAudio);
        }
        else
        {
            fails++;
            AudioManager.Instance.PlayOnShot(failAudio);
        }
        if (UIPoints != null)
        {
            UIPoints.text =(asserts-fails).ToString()+ " puntos";
        }
        if (asserts >= pieces.Length)
        {
            EndGame();
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
        if (pieces.Length <= asserts)
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
                selectedPiece=hit.collider.gameObject;
                if(DificultManager.Instance.DificultLevel==DificultLevel.HARD)
                {
                    NextMove();
                    return;
                }
                FadeButtons(true);
                if (hit.collider.gameObject.TryGetComponent<Touchable>(out var touchable))
                {
                    touchable.MakeItGlow(true);
                }

            }
        }
    }
}
