using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(AudioListener))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip correct;
    [SerializeField]
    AudioClip fail;
    [SerializeField]
    List<AudioClip> background;
    static AudioSource m_AudioSource;
    bool stopMusic;
    bool disabledEffects;
    public bool StopMusic
    {
        set { 
            if (m_AudioSource.isPlaying) {
                m_AudioSource.Stop();
            } 
            stopMusic = value; 
        }
    }
    public bool DisabledEffects
    {
        set
        {
            disabledEffects = value;
        }
    }
    private AudioManager() { }
    public static AudioManager Instance
    {
        get
        {
            return Singleton<AudioManager>.Instance;
        }
    }
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    

    public void PlayOnShot(AudioClip clip)
    {
        if (disabledEffects)
        {
            return;
        }
        if(clip == null)
        {
            return;
        }
        m_AudioSource.PlayOneShot(clip);
    }
    public void CorrectPlay(bool state)
    {
        PlayOnShot(state?correct:fail);
    }
    public void AudioLevel(float level)
    {
        m_AudioSource.volume = level;
    }
    private void Update()
    {
        if(!stopMusic && m_AudioSource != null)
        {
            if(!m_AudioSource.isPlaying)
            {
                PlayNextClip();
            }
        }
    }
    private void PlayNextClip()
    {
        if (background.Count == 0)
        {
            return;
        }
        int index=background.Count==1?0: Random.Range(0, background.Count);
        m_AudioSource.clip = background[index];
        m_AudioSource.Play();
    }
}
