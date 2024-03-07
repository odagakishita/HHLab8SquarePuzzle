using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip destroysound;
    public AudioClip destroy2sound;
    public AudioClip falledsound;
    public AudioClip lightsound;
    //new AudioSource audio;
    public static SoundManager instance;

    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;
    //[SerializeField] AudioSource seAudioSource;
    //void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public void SetBGMVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void SetSEVolume(float volume)
    {
        
        seAudioSource.volume = volume;
    }

    public void LightSound()
    {
        seAudioSource.PlayOneShot(lightsound, 1.0f);
    }

    public void DestroySound()
    {
        seAudioSource.PlayOneShot(destroysound,0.7f);
    }
    public void Destroy2Sound()
    {
        seAudioSource.PlayOneShot(destroy2sound, 0.7f);
    }
    public void FalledSound()
    {
        seAudioSource.PlayOneShot(falledsound);
    }
}
