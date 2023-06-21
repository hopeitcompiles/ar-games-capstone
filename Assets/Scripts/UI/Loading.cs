using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    GameObject loadingPanel;
    Tweener rotationTweener;
    
    public static Loading instance;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
        SetLoading(false);
    }
    public void SetLoading(bool state, string message="Cargando")
    {
        Image loadingImage = loadingPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        TextMeshProUGUI loadingText = loadingPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        loadingText.text = message;
        loadingPanel.SetActive(state);
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        transform.DOScale(scale, 0.3f);
        gameObject.SetActive(state);
        if(state)
        {
            rotationTweener= loadingImage.transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.InOutCirc);
        }
        else
        {
            rotationTweener.Kill();
        }
    }

}
