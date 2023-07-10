using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Configuration : MonoBehaviour
{
    [SerializeField]
    GameObject panel; 
    [SerializeField]
    Slider volume; 
    [SerializeField]
    Slider drag;
    [SerializeField]
    TextMeshProUGUI dragText;
    [SerializeField]
    Image uiDisabledMusic;
    bool disabledMusic;

    [SerializeField]
    Image uiDisabledEffects;
    bool disabledEffects;

    [SerializeField]
    Image uiDisabledVibration;
    bool disabledVibration;

    private float dragSpeed = 0f;
    public static Configuration instance;
    private int[] fpsSteps = { 30, 60,90,100,120,144};
    public float DragSpeed
    {
        get { return dragSpeed; }
    }
    
    void Awake()
    {
        instance = this;
        panel.SetActive(false);
        panel.transform.localScale = Vector3.zero;
        volume=panel.GetComponentInChildren<Slider>();
        volume.onValueChanged.AddListener(ChangeVolume);
        drag.onValueChanged.AddListener(ChangeDrag);
        disabledMusic = true;
        disabledEffects = true;
        disabledVibration = true;
    }

    private void ChangeDrag(float speed)
    {
        dragSpeed = speed;
        dragText.text = "Sensibilidad "+(dragSpeed*100).ToString("0")+"%";
    }

    private void Start()
    {
        string _volume = PlayerPrefs.GetString("volume");
        if (_volume != null && _volume != "")
        {
            volume.value = float.Parse(_volume);
        }
        
        string _disabledMusic = PlayerPrefs.GetString("music");
        if (_disabledMusic != null && _disabledMusic != "")
        {
            disabledMusic = !bool.Parse(_disabledMusic);
        }
        
        string _disabledEffects = PlayerPrefs.GetString("effects");
        if (_disabledEffects != null && _disabledEffects != "")
        {
            disabledEffects = !bool.Parse(_disabledEffects);
        }

        string _disabledVibration = PlayerPrefs.GetString("vibration");
        if (_disabledVibration != null && _disabledVibration!="")
        {
            disabledVibration = !bool.Parse(_disabledVibration);
        }

      
        string _drag = PlayerPrefs.GetString("drag");
        if (_drag != null && _drag != "")
        {
            dragSpeed = float.Parse(_drag);
        }
        else
        {
            dragSpeed = 1f;
        }
        drag.value = dragSpeed;
        ChangeDrag(dragSpeed);
        DisabledMusic();
        DisabledEffects();
        DisabledVibration();
        GameManager.Instance.OnARPosition += Instance_OnARPosition;
        GameManager.Instance.OnMainMenu += Instance_OnMainMenu;
        GameManager.Instance.OnGame += Instance_OnMainMenu;
    }

    private void Instance_OnMainMenu()
    {
        AudioManager.Instance.StopMusic = disabledMusic;
    }

    private void Instance_OnARPosition()
    {
        AudioManager.Instance.StopMusic = true;
    }

    public void DisabledMusic()
    {
        disabledMusic = !disabledMusic;
        uiDisabledMusic.gameObject.SetActive(disabledMusic);
        AudioManager.Instance.StopMusic=disabledMusic;
    }
    public void DisabledEffects()
    {
        disabledEffects = !disabledEffects;
        uiDisabledEffects.gameObject.SetActive(disabledEffects);
        AudioManager.Instance.DisabledEffects = disabledEffects;
    }

    public void DisabledVibration()
    {
        disabledVibration = !disabledVibration;
        uiDisabledVibration.gameObject.SetActive(disabledVibration);
        Vibration.instance.VibrateDisabled = disabledVibration;
    }

    public void ChangeVolume(float value)
    {
        AudioManager.Instance.AudioLevel(value);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetString("volume",volume.value.ToString());
        PlayerPrefs.SetString("drag",drag.value.ToString());
        PlayerPrefs.SetString("music",disabledMusic.ToString());
        PlayerPrefs.SetString("effects",disabledMusic.ToString());
    }
    // Update is called once per frame
    public void ShowConfigPanel(bool state)
    {
        if(state && !panel.activeSelf)
        {
            panel.SetActive(true);
        }
        panel.transform.DOScale(state ? Vector3.one : Vector3.zero, 0.3f).SetUpdate(true);
    }

    public void ChangeFPSLimit(float value)
    {
        //fpsText.text ="Límite " +fpsSteps[(int)value]+" FPS";
        LimitFPS.instance.TargetFrameRate(fpsSteps[(int)value]);
    }
}
