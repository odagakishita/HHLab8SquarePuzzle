using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip destroysound;
    public AudioClip destroy2sound;
    public AudioClip falledsound;
    new AudioSource audio;

    public void AudioInit()
    {
        audio = GetComponent<AudioSource>();
    }

    public void DestroySound()
    {
        audio.PlayOneShot(destroysound,0.5f);
    }
    public void Destroy2Sound()
    {
        audio.PlayOneShot(destroy2sound, 0.5f);
    }
    public void FalledSound()
    {
        audio.PlayOneShot(falledsound);
    }
}
