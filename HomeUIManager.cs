using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeUIManager : MonoBehaviour {

    public GameObject parentLoading, btnPlay, bgPlay;
    public GameObject LevelSelect;
    
    public Animator animLevelSelect;

    // Use this for initialization
    void Start() {
        Time.timeScale = 1;

        GPGS.Login();

        startAnim();
    }

    // Update is called once per frame
    void Update() {
    }

    public void OnClick(string button)
    {
        switch (button)
        {
            case "Play":
                LevelSelect.SetActive(true);
                btnPlay.SetActive(false);
                bgPlay.SetActive(false);
                animLevelSelect.SetFloat("Speed", 1);
                break;
            case "Back":
                animLevelSelect.SetFloat("Speed", -1);
                break;
            case "Setting":
                SceneManager.LoadScene("Setting");
                break;
            case "Exit":
                Application.Quit();
                break;
            case "GPGS":
                GPGS.Login();
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
    }

    public void startAnim()
    {
        animLevelSelect.SetFloat("Speed", 0);
        if (!btnPlay.activeInHierarchy)
        {
            btnPlay.SetActive(true);
            bgPlay.SetActive(true);
        }
        LevelSelect.SetActive(false);
        UnityEngine.Debug.Log("Start.");
    }
}
