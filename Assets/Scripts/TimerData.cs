using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TimerData : MonoBehaviour
{
    
    public void TimeInit(TextMeshProUGUI uiText, float CountTime, float time)
    {
        //Debug.Log("hogehoge");
        float timer = CountTime;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        uiText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        //uiFill.fillAmount = 1;
        
    }
    public float TimeCount(TextMeshProUGUI uiText, float CountTime, float time)
    {
        //Debug.Log(CountTime);
        CountTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(CountTime / 60);
        int seconds = Mathf.FloorToInt(CountTime % 60);
        
        uiText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        if (CountTime < 1)
        {
            SendMessage("SoloSceneMovetoResult");
            //Debug.Log("a");
        }
        return CountTime;

    }
}