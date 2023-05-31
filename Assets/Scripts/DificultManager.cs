using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DificultManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private TextMeshProUGUI levelText;

    private DificultLevel dificulty;

    public static DificultManager Instance
    {
        get { return Singleton<DificultManager>.Instance; }
    }
    public DificultLevel DificultLevel
    {
        get { return dificulty; }
    }
    
    private DificultManager() { }

    // Start is called before the first frame update
    void Awake()
    {
        InitializaDificult();
        if(slider != null)
        {
            slider.onValueChanged.AddListener((float value)=> DificultChanged(value));
        }
        levelText=slider.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        DificultChanged(slider.value);
    }
    public void DificultChanged(float value)
    {
        switch (value)
        {
            case 0:
                {
                    levelText.text = "Nivel Fácil";
                    levelText.color = Color.green;
                    dificulty = DificultLevel.EASY;
                    break; ;
                }
            case 1:
                {
                    levelText.text = "Nivel Medio";
                    levelText.color = Color.yellow;
                    dificulty = DificultLevel.MEDIUM;
                    break; ;
                }
            case 2:
                {
                    levelText.text = "Nivel Difícil";
                    levelText.color = Color.red;
                    dificulty = DificultLevel.HARD;
                    break; ;
                }
        }
    }
    private void InitializaDificult()
    {
        dificulty = DificultLevel.MEDIUM;
    }
    public void ChangeDificult(DificultLevel dificulty)
    {
        this.dificulty = dificulty;
    }
}
