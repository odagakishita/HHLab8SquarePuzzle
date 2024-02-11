using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resultSound : MonoBehaviour
{
    public AudioClip buttonclick;
    new AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void ButtonSound()
    {
        audio.PlayOneShot(buttonclick);
    }
}
