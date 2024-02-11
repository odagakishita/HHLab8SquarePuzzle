using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choiceAnimation : MonoBehaviour
{
    public bool choice1click;
    public bool choice2click;
    void Start()
    {
        choice1click = false;
        choice2click = false;

    }

    public void AnimationFinished()
    {
        choice1click = true;
        choice2click = true;
    }
    public void Animationstart()
    {
        choice1click = false;
        choice2click = false;
    }
}
