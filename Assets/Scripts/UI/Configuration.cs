using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Configuration : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    Slider volume;

    [SerializeField]
    Image uiDisabledMusic;
    bool disabledMusic;

    [SerializeField]
    Image uiDisabledEffects;
    bool disabledEffects;

    [SerializeField]
    Image uiDisabledVibration;
    bool disabledVibration;

    void Awake()
    {
        panel.SetActive(false);
        panel.transform.localScale = Vector3.zero;
        volume=panel.GetComponentInChildren<Slider>();
        volume.onValueChanged.AddListener(ChangeVolume);
        disabledMusic = true;
        disabledEffects = true;
        disabledVibration = true;
    }
    private void Start()
    {
        string _volume = PlayerPrefs.GetString("volume");
        if (_volume != null)
        {
            volume.value = float.Parse(_volume);
        }
        else
        {
            volume.value = 1;
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
}
