using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum Result
{
    GOOD,OK,BAD, IMCOMPLETE
}
public class ResultsManager : MonoBehaviour
{
    [SerializeField]
    private Results results;
    [SerializeField]
    private Sprite goldMedal;
    [SerializeField]
    private Sprite silverMedal;
    [SerializeField]
    private Sprite bronzeMedal;
    [SerializeField]
    private Sprite failMedal;
    
    [SerializeField]
    private AudioClip audioOk;    
    [SerializeField]
    private AudioClip audioGood;    
    [SerializeField]
    private AudioClip audioBad;

    [SerializeField]
    TMP_InputField comments;
    [SerializeField]
    Button sendStatsButton;
    [SerializeField]
    TextMeshProUGUI error;
    Models.GameMetric metrics;


    private static ResultsManager instance;
    public static ResultsManager Instance
    {
        get { return instance; }
    }
  
    void Awake()
    {
        instance = this;
        GameManager.Instance.OnMainMenu += Reset;
        GameManager.Instance.OnARPosition += Reset;
        results.transform.localScale=Vector3.zero;
        sendStatsButton.onClick.AddListener(SendMetrics);
    }

    private void Reset()
    {
        results.Stats = null;
        results.transform.localScale = Vector3.zero;
    }

    public void Activate(bool state, Result result, Models.GameMetric metric)
    {
        Models.ProfileData user = Profile.instance.User;
        if (user == null || user.role!=Role.STUDENT.ToString())
        {
            comments.transform.parent.gameObject.SetActive(false);
            sendStatsButton.gameObject.SetActive(false);
            if (user == null)
            {
                error.text = "Inicia sesión y únete a una clase para enviar tus resultados a tu docente";
            }
            else
            {
                error.text = user.role == Role.TEACHER.ToString() ? "Envía tus resultados a tu docente... Espera, tú eres el docente." 
                    : "Administradores no pueden enviar sus estadísticas";
            }
        }
        else
        {
            comments.transform.parent.gameObject.SetActive(true);
            sendStatsButton.gameObject.SetActive(true);
            sendStatsButton.interactable = true;
        }
        metrics=metric;
        Vector3 scale = state ? Vector3.one : Vector3.zero;
        results.gameObject.SetActive(state);
        results.transform.DOScale(scale, 0.3f).SetUpdate(true);
        if(!state)
        {
            return;
        }
        results.Stats = metric;
        if (result==Result.GOOD && audioGood != null)
        {
            AudioManager.Instance.PlayOnShot(audioGood);
            results.Medal = silverMedal;
            return;
        }
        if (result == Result.BAD && audioBad != null)
        {
            AudioManager.Instance.PlayOnShot(audioBad);
            results.Medal = bronzeMedal;
            return;
        }
        if (result == Result.IMCOMPLETE && audioBad != null)
        {
            AudioManager.Instance.PlayOnShot(audioBad);
            results.Medal = failMedal;
            return;
        }
        if (audioOk!=null)
        {
            AudioManager.Instance.PlayOnShot(audioOk);
            results.Medal = goldMedal;
        }
    }
    public async void SendMetrics()
    {
        sendStatsButton.interactable = false;
        metrics.comments = comments.text;
        string value=await GameStatsSender.instance.SendStats(metrics);
        error.text= value;
    }
}
