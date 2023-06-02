using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(AudioListener))]
public class AudioManager : MonoBehaviour
{
    static AudioSource m_AudioSource;

    private AudioManager() { }
    public static AudioManager Instance
    {
        get
        {
            return Singleton<AudioManager>.Instance;
        }
    }
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayOnShot(AudioClip clip)
    {
        if(clip == null)
        {
            return;
        }
        m_AudioSource.PlayOneShot(clip);
    }
}
