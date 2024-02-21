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
        //result.ButtonSound();
        SceneManager.LoadScene("start");
        
    }


   

    public void SoloSceneMove()
    {
        //Animator anim = choice2.GetComponent<Animator>();
        if (button.choice1click) SceneManager.LoadScene("Main");
        else if(SceneManager.GetActiveScene().name == "result")
        {
            SceneManager.LoadScene("MainEasyMode");
            
        }
        else if (SceneManager.GetActiveScene().name == "result2")
        {
            SceneManager.LoadScene("Main");

        }
        //result.ButtonSound();

    }

    public void SoloSceneMoveToEasy()
    {
        //Animator anim = choice1.GetComponent<Animator>();
        if(button.choice2click) SceneManager.LoadScene("MainEasyMode");
        //result.ButtonSound();

    }

    public void SoloSceneMovetoResult()
    {
        if(SquareManager.ballnumbers == 4) SceneManager.LoadScene("result");
        else SceneManager.LoadScene("result2");
        // result.ButtonSound();
    }


}
