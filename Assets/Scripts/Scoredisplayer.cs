using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using unityroom.Api;
using UnityEngine.UI;

public class Scoredisplayer : MonoBehaviour
{
    public Image Retry;
    public Image Home;
    //public GameObject Rank;
    Animator Retryanim;
    Animator Homeanim;
    //Animator Rankanim;
    public TextMeshProUGUI resultscore;
    float score;
    // Start is called before the first frame update
    void Start()
    {
        
        
        score = Judgemanager.getscore();
        resultscore.text = score.ToString();
        if (SquareManager.ballnumbers == 4) UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
        else if (SquareManager.ballnumbers == 5) UnityroomApiClient.Instance.SendScore(2, score, ScoreboardWriteMode.HighScoreDesc);
        Retryanim = Retry.GetComponent<Animator>();
        Homeanim = Home.GetComponent<Animator>();
       // Rankanim = Rank.GetComponent<Animator>();
        //Unity
    }

    public void RetryEnter()
    {
        Retryanim.SetBool("MouseEnter",true);
    }
    public void RetryExit()
    {
        Retryanim.SetBool("MouseEnter", false);
    }

    public void HomeEnter()
    {
        Homeanim.SetBool("HomeEnter", true);
    }
    public void HomeExit()
    {
        Homeanim.SetBool("HomeEnter", false);
    }
    //public void RankEnter()
    //{
    //    Rankanim.SetBool("RankEnter", true);
    //}
    //public void RankExit()
    //{
    //    Rankanim.SetBool("RankEnter", false);
    //}

    public void RankDown()
    {
        UnityroomApiClient.Instance.SendScore(1,score,ScoreboardWriteMode.HighScoreDesc);
    }

}
