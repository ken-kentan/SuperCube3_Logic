using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingUIManager : MonoBehaviour {
    
    public Toggle toggleGyro, toggleVib, toggleBlur;
    public Slider sliderGyro;
    public Text infoGyro, infoCtrl;

    private static bool isController;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("isController", 0) == 0) toggleGyro.isOn = true;

        if (PlayerPrefs.GetInt("isVibration", 1) == 1)  toggleVib.isOn = true;
        if (PlayerPrefs.GetInt("isBlur", 1)      == 1) toggleBlur.isOn = true;

        sliderGyro.value = PlayerPrefs.GetFloat("KaccGyro", 25.0f) / 25.0f;

        if (Msg.isLangJa)
        {
            infoGyro.text = Msg.jaSetting[0];
            infoCtrl.text = Msg.jaSetting[1];
        }
        else
        {
            infoGyro.text = Msg.enSetting[0];
            infoCtrl.text = Msg.enSetting[1];
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void isToggleGyro()
    {
        if(!toggleGyro.isOn) isController = true;
        else                 isController = false;

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
        }
    }

    public void onClickReset()
    {
        sliderGyro.value = 1.0f;
        toggleGyro.isOn  = true;
        toggleVib.isOn   = true;
        toggleBlur.isOn  = true;
    }

    public void onClickBack()
    {
        PlayerPrefs.SetFloat("KaccGyro", sliderGyro.value * 25.0f);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Home");
    }
}
