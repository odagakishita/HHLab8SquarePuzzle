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
    //[SerializeField] AudioSource seAudioSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void LightSound()
    {
        bgmAudioSource.PlayOneShot(lightsound, 1.0f);
    }

    public void DestroySound()
    {
        bgmAudioSource.PlayOneShot(destroysound,0.7f);
    }
    public void Destroy2Sound()
    {
        bgmAudioSource.PlayOneShot(destroy2sound, 0.7f);
    }
    public void FalledSound()
    {
        bgmAudioSource.PlayOneShot(falledsound);
    }
}
