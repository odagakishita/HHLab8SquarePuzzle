using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    public GameObject SettingPanel;
    public GameObject VolumeSlider;
    Animator SetAnim;

    public bool IsGameStop;
    public void SettingsButton()
    {
        
        SetAnim = SettingPanel.GetComponent<Animator>();
        SettingPanel.SetActive(true);
        SetAnim.SetBool("Panel", true);
        
    }
    public void SettingsButtonExit()
    {

        //SetAnim = SettingPanel.GetComponent<Animator>();
        SettingPanel.SetActive(false);
        Time.timeScale = 1f;


    }

    public void SettingsVolume()
    {

        VolumeSlider.SetActive(true);
    }

    public void SettingsVolumeExit()
    {

        VolumeSlider.SetActive(false);
    }

    public void GameStopping()
    {
        IsGameStop = true;
        Time.timeScale = 0f;
    }

    public void SetBGMVolume(float volume)
    {
        SoundManager.instance.SetBGMVolume(volume);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
