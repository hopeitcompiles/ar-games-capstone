using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
   
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    TextMeshProUGUI timeElapsed;
    [SerializeField]
    TextMeshProUGUI percentage;
    [SerializeField]
    TextMeshProUGUI successCount;
    [SerializeField]
    TextMeshProUGUI failureCount;
    
   
    [SerializeField]
    private Image medal;


    public Sprite Medal
    {
        set { medal.sprite = value; }
    }
    public Models.GameMetric Stats
    {
        set {if (value == null) { return; }
            score.text = value.score.ToString(value.score % 1 == 0 ? "0" : "0.00") + (value.score == 1 ? " punto" : " puntos");
            timeElapsed.text = value.timeElapsed.ToString(value.timeElapsed % 1 == 0 ? "0" : "0.00") + (value.timeElapsed == 1 ? " segundo" : " segundos");
            percentage.text = value.percentageOfCompletion.ToString(value.percentageOfCompletion % 1 == 0 ? "0" : "0.00") + " % completado";
            successCount.text = value.successCount.ToString() + (value.successCount == 1 ? " acierto" : " aciertos");
            failureCount.text = value.failureCount.ToString() + (value.failureCount == 1 ? " error" : " errores");
        }
    }

}
