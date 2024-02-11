using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    new AudioSource audio;
    public AudioClip openbutton;
    public Image choice1;
    public Image choice2;
    private Animator anim1;
    private Animator anim2;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        anim1 = choice1.GetComponent<Animator>();
        anim2 = choice2.GetComponent<Animator>();
    }

    
    
    public void MouseEnter()
    {
        audio.PlayOneShot(openbutton);
        anim1.SetBool("easymove",true);
        anim2.SetBool("hardmove", true);
        //Debug.Log("a");
    }
    public void MouseExit()
    {
        anim1.SetBool("easymove", false);
        anim2.SetBool("hardmove", false);
        //Debug.Log("a");
    }
    public void choice1Enter()
    {
        anim1.SetBool("big", true);
        //Debug.Log("a");
    }
    public void choice1Exit()
    {
        anim1.SetBool("big", false);
        //Debug.Log("a");
    }

    public void choice2Enter()
    {
        anim2.SetBool("big", true);
        //Debug.Log("a");
    }
    public void choice2Exit()
    {
        anim2.SetBool("big", false);
        //Debug.Log("a");
    }

}
