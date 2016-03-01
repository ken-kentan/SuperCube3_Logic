using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingUIManager : MonoBehaviour {
    
    public Toggle toggleGyro, toggleVib, toggleBlur, toggleFPS, toggleDebug;
    public Slider sliderGyro;
    public Text infoGyro, infoCtrl;

    private static bool isController;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("isController", 0) == 0) toggleGyro.isOn  = true;
        if (PlayerPrefs.GetInt("isVibration",  1) == 1) toggleVib.isOn   = true;
        if (PlayerPrefs.GetInt("isBlur",       1) == 1) toggleBlur.isOn  = true;
        if (PlayerPrefs.GetInt("isDisplayFPS", 0) == 1) toggleFPS.isOn   = true;
        if (PlayerPrefs.GetInt("isDebug",      0) == 1) toggleDebug.isOn = true;

        sliderGyro.value = PlayerPrefs.GetFloat("KaccGyro", 25.0f) / 25.0f;

        infoGyro.text = Msg.Setting[Msg.typeLang, 0];
        infoCtrl.text = Msg.Setting[Msg.typeLang, 1];

        World.isPause = false;
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void isToggleGyro()
    {
        if(!toggleGyro.isOn) isController = true;
        else                 isController = false;

        World.isController = isController;
        PlayerPrefs.SetInt("isController", System.Convert.ToInt32(isController));
    }

    public void onValueChanged(string toggle)
    {
        switch (toggle)
        {
            case "Vibration":
                PlayerPrefs.SetInt("isVibration", System.Convert.ToInt32(toggleVib.isOn));
                break;
            case "Blur":
                PlayerPrefs.SetInt("isBlur", System.Convert.ToInt32(toggleBlur.isOn));
                break;
            case "FPS":
                PlayerPrefs.SetInt("isDisplayFPS", System.Convert.ToInt32(toggleFPS.isOn));
                break;
            case "Debug":
                PlayerPrefs.SetInt("isDebug", System.Convert.ToInt32(toggleDebug.isOn));
                break;
        }
    }

    public void onValueChangedGyro()
    {
        CubeManager.KaccGyro = sliderGyro.value * 25.0f;
    }

    public void onClickReset()
    {
        sliderGyro.value = 1.0f;
        toggleGyro.isOn  = true;
        toggleVib.isOn   = true;
        toggleBlur.isOn  = true;

        toggleFPS.isOn  = false;
        toggleDebug.isOn = false;
    }

    public void onClickBack()
    {
        PlayerPrefs.SetFloat("KaccGyro", sliderGyro.value * 25.0f);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Home");
    }
}
