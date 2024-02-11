using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultipleSceneManager : MonoBehaviour
{
    [SerializeField]
    choiceAnimation button;

    public Image choice1;
    public Image choice2;
    // Update is called once per frame
    
    public void SoloSceneStart()
    {
        SceneManager.LoadScene("start");
    }


   

    public void SoloSceneMove()
    {
        //Animator anim = choice2.GetComponent<Animator>();
        if (button.choice1click) SceneManager.LoadScene("Main");
        else if(SceneManager.GetActiveScene().name == "result")
        {
            if(SquareManager.ballnumbers ==4) SceneManager.LoadScene("MainEasyMode");
            else if (SquareManager.ballnumbers ==5) SceneManager.LoadScene("Main");
        }

    }

    public void SoloSceneMoveToEasy()
    {
        //Animator anim = choice1.GetComponent<Animator>();
        if(button.choice2click) SceneManager.LoadScene("MainEasyMode");

    }

    public void SoloSceneMovetoResult()
    {
        SceneManager.LoadScene("result");
    }


}
