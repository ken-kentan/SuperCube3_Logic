using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingUIManager : MonoBehaviour {
    
    public Toggle toggleGyro, toggleVib, toggleBlur, toggleFPS, toggleHQ;
    public Slider sliderGyro;
    public Text infoGyro, infoCtrl;
    public GameObject objFPS, Controller;

    private bool isController;
    private bool isLock;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("isController",  1) == 0) toggleGyro.isOn  = true;
        if (PlayerPrefs.GetInt("isVibration",   1) == 1) toggleVib.isOn   = true;
        if (PlayerPrefs.GetInt("isBlur",        1) == 1) toggleBlur.isOn  = true;
        if (PlayerPrefs.GetInt("isDisplayFPS",  0) == 1) toggleFPS.isOn   = true;
        if (PlayerPrefs.GetInt("isHighQuality", 1) == 1) toggleHQ.isOn    = true;

        if (World.isController) Controller.SetActive(true);

        sliderGyro.value = PlayerPrefs.GetFloat("KaccGyro", 25.0f) / 25.0f;

        infoGyro.text = Msg.Setting[Msg.typeLang, 0];
        infoCtrl.text = Msg.Setting[Msg.typeLang, 1];

        World.isPause = false;
    }

    public void IsToggleGyro()
    {
        if(!toggleGyro.isOn) isController = true;
        else                 isController = false;

        World.isController = isController;
        PlayerPrefs.SetInt("isController", System.Convert.ToInt32(isController));

        Controller.SetActive(isController);
    }

    public void IsToggleHQ()
    {
        if(toggleHQ.isOn) QualitySettings.SetQualityLevel(2, true);
        else              QualitySettings.SetQualityLevel(0, true);

        PlayerPrefs.SetInt("isHighQuality", System.Convert.ToInt32(toggleHQ.isOn));
    }

    public void OnValueChanged(string type)
    {
        switch (type)
        {
            case "Vibration":
                PlayerPrefs.SetInt("isVibration", System.Convert.ToInt32(toggleVib.isOn));
                break;
            case "Blur":
                PlayerPrefs.SetInt("isBlur", System.Convert.ToInt32(toggleBlur.isOn));
                break;
            case "FPS":
                PlayerPrefs.SetInt("isDisplayFPS", System.Convert.ToInt32(toggleFPS.isOn));
                objFPS.SetActive(toggleFPS.isOn);
                break;
        }
    }

    public void OnValueChangedGyro()
    {
        CubeManager.KaccGyro = sliderGyro.value * 25.0f;
    }

    public void OnPressJump(bool isPress)
    {
        if (Loading.isLoading || CubeManager.isWarpLock) return;

        if (isLock)
        {
            if (!isPress) isLock = false;
            return;
        }

        if (isPress) isLock = true;

        GameUIManager.isJump = true;
    }

    public void onClickReset()
    {
        sliderGyro.value = 1.0f;
        toggleGyro.isOn  = true;
        toggleVib.isOn   = true;
        toggleBlur.isOn  = true;
        toggleHQ.isOn    = true;

        toggleFPS.isOn  = false;
    }

    public void onClickBack()
    {
        PlayerPrefs.SetFloat("KaccGyro", sliderGyro.value * 25.0f);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Home");
    }
}
