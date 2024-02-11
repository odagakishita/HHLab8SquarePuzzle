using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip falledsound;
    new AudioSource audio;

    public void AudioInit()
    {
        audio = GetComponent<AudioSource>();
    }

    public void FalledSound()
    {
        audio.PlayOneShot(falledsound);
    }
}
