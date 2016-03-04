using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeUIManager : MonoBehaviour {

    public GameObject parentLoading, btnPlay, bgPlay, btnData, bgData;
    public GameObject LevelSelect, Data;
    public Text textScore, textJump, textClear, textSave, textPoint, textPlusOne, textMagnet, textDead, textKill, textSBlock, textSRoute;
    
    public Animator animLevelSelect;
    private bool isFirst;

    // Use this for initialization
    void Start() {
        Time.timeScale = 1;

        GPGS.Login();

        isFirst = true;

        animLevelSelect.SetFloat("Speed", 0);
    }

    // Update is called once per frame
    void Update() {
    }

    void setGameData()
    {
        textScore.text = GameDataManager.Score.ToString();
        textJump.text  = GameDataManager.Jump.ToString();
        textClear.text = GameDataManager.Clear.ToString();
        textSave.text  = GameDataManager.Save.ToString();

        textPoint.text   = GameDataManager.Point.ToString();
        textPlusOne.text = GameDataManager.Aqua.ToString();
        textMagnet.text  = GameDataManager.Magnet.ToString();

        textDead.text = GameDataManager.Dead.ToString();
        textKill.text = GameDataManager.Kill.ToString();

        textSBlock.text = GameDataManager.SecretBlock.ToString();
        textSRoute.text = GameDataManager.SecretRoute.ToString();
    }

    public void OnClick(string button)
    {
        switch (button)
        {
            case "Play":
                isFirst = true;
                LevelSelect.SetActive(true);
                btnPlay.SetActive(false);
                bgPlay.SetActive(false);
                animLevelSelect.Play("openLevelSelect");
                animLevelSelect.SetFloat("Speed", 1);
                break;
            case "Back":
                animLevelSelect.SetFloat("Speed", -1);
                break;
            case "Data":
                isFirst = true;
                Data.SetActive(true);
                btnData.SetActive(false);
                bgData.SetActive(false);
                animLevelSelect.Play("openGameData");
                animLevelSelect.SetFloat("Speed", 1);
                setGameData();
                break;
            case "Setting":
                SceneManager.LoadScene("Setting");
                break;
            case "Feedback":
                Application.OpenURL("https://twitter.com/intent/tweet?screen_name=ken_kentan");
                break;
            case "Exit":
                Application.Quit();
                break;
            case "GPGS":
                GPGS.Login();
                break;
            case "@ken":
                Application.OpenURL("https://twitter.com/ken_kentan");
                break;
            case "@kazu":
                Application.OpenURL("https://twitter.com/kazuchikatch");
                break;
            case "Maou":
                Application.OpenURL("http://maoudamashii.jokersounds.com/");
                break;
        }
    }

    public void OnClickLevel(string level)
    {
        parentLoading.SetActive(true);
        SceneManager.LoadSceneAsync(level);
    }

    public void endAnim() {
        animLevelSelect.SetFloat("Speed", 0);
        UnityEngine.Debug.Log("End.");
        isFirst = false;
    }

    public void startAnim(string modeAnim)
    {
        if (isFirst) return;

        switch (modeAnim)
        {
            case "LevelSelect":
                if (!btnPlay.activeInHierarchy)
                {
                    btnPlay.SetActive(true);
                    bgPlay.SetActive(true);
                    animLevelSelect.SetFloat("Speed", 0);
                    LevelSelect.SetActive(false);
                    UnityEngine.Debug.Log("LS Start.");
                }
                break;
            case "GameData":
                if (!btnData.activeInHierarchy)
                {
                    btnData.SetActive(true);
                    bgData.SetActive(true);
                    animLevelSelect.SetFloat("Speed", 0);
                    Data.SetActive(false);
                    UnityEngine.Debug.Log("GD Start.");
                }
                break;
        }
        UnityEngine.Debug.Log("Start.");
    }
}
