using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUIManager : MonoBehaviour {

    public Text Clear;
    public GameObject Paused, GameOver;
    public UnityStandardAssets.ImageEffects.BlurOptimized Blur;

    private static int cntDelay;
    private float animationBlur;
    private int modeAnimBlur; //0:none 1:Enable 2:Disable

	// Use this for initialization
	void Start () {
        cntDelay = 0;
        animationBlur = 0.0f;
        modeAnimBlur = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //Clear
        if (Goal.isEnterCube)
        {
            World.isPause = true;
            Time.timeScale = 0;
            Clear.enabled = true;
        }

        //Pause
        if (Input.GetKey(KeyCode.Escape) && cntDelay == 0 && !Goal.isEnterCube)
        {
            cntDelay = 1;
            World.isPause = !World.isPause;

            if (Time.timeScale != 0)
            {
                World.audioVolume(0.3f);
                enablePause();
                Paused.SetActive(true);
            }
            else {
                World.audioVolume(1.0f);
                disablePause();
            }
        }

        //GameOver
        if (World.isGameOver)
        {
            World.isPause = true;
            World.audioVolume(0.0f);
            enablePause();
            GameOver.SetActive(true);
        }

        switch (modeAnimBlur)
        {
            case 1:
                Blur.blurSize = animationBlur;

                if (animationBlur >= 2.0f)
                {
                    animationBlur = 2.0f;
                    modeAnimBlur = 0;
                }
                else {
                    animationBlur += 0.2f;
                    Paused.transform.localPosition -= new Vector3(80f, 0, 0);
                }
                break;
            case 2:
                Blur.blurSize = animationBlur;

                if (animationBlur <= 0.0f)
                {
                    animationBlur = 0.0f;
                    modeAnimBlur = 0;
                    Blur.enabled = false;
                    Paused.SetActive(false);
                }
                else {
                    animationBlur -= 0.2f;
                    Paused.transform.localPosition += new Vector3(80f, 0, 0);
                }
                break;
        }

        cntTimer();
    }

    void cntTimer()
    {
        if (cntDelay == 0) return;

        cntDelay++;
        if (cntDelay > 20) cntDelay = 0;
    }

    void enablePause()
    {
        modeAnimBlur = 1;
        Blur.enabled = true;
        Time.timeScale = 0;
    }

    void disablePause()
    {
        modeAnimBlur = 2;
        Time.timeScale = 1;
    }

    public void OnClick(string button)
    {
        UnityEngine.Debug.Log(button + " button cliked.");

        switch (button)
        {
            case "Home":
                SceneManager.LoadScene("Home");
                break;
            case "Resum":
                World.isPause = false;
                cntDelay = 1;
                World.audioVolume(1.0f);
                disablePause();
                break;
            case "Retry":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            default:
                break;
        }
    }
}
