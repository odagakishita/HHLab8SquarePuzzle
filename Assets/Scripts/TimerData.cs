using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerData : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private float CountTime;

    private void Update()
    {
        float timer = CountTime - Time.time;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        uiFill.fillAmount = Mathf.InverseLerp(0, CountTime, timer);
        uiText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}