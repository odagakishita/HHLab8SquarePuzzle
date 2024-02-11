using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoredisplayer : MonoBehaviour
{
    public TextMeshProUGUI resultscore;
    float score;
    // Start is called before the first frame update
    void Start()
    {
        score = Judgemanager.getscore();
        resultscore.text = score.ToString();
    }

    
}
