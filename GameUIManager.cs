using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUIManager : MonoBehaviour {

    public Text Hint, Score, infoRevival;
    public GameObject Paused, GameOver, Clear, Controller;
    public GameObject btnRetry, btnRevival, btnHome;
    public UnityStandardAssets.ImageEffects.BlurOptimized Blur;

    public static bool isLeft, isRight, isJump;

    private static int cntDelay, cntRevival;
    private float animationBlur;
    private int modeAnimBlur; //0:none 1:Enable 2:Disable
    private static bool isLock;

	// Use this for initialization
	void Start () {
        cntDelay = cntRevival = 0;
        animationBlur = 0.0f;
        modeAnimBlur = 0;

        isLeft = isRight = isJump = false;

        if (World.isController) Controller.SetActive(true);

        isLock = false;

        if (Msg.isLangJa) infoRevival.text = Msg.jaRevival;
        else              infoRevival.text = Msg.enRevival;
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isLoading) return;

        //Clear
        if (Goal.isEnterCube)
        {
            World.isPause = true;
            enablePause();
            Clear.SetActive(true);
            if (World.sumScore == 0)
            {
                World.audioVolume(0.0f);
                World.calcScore();
            }
            Score.text = World.sumScore.ToString();
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
            if (cntRevival >= 2) disableRevival();
            GameOver.SetActive(true);
            if (Hint != null)
            {
                World.audioVolume(0.0f);

                if (Msg.isLangJa) Hint.text = Msg.jaHint[generateRand(0, 7)];
                else              Hint.text = Msg.enHint[generateRand(0, 7)];

                Hint = null;
            }
        }

        switch (modeAnimBlur)
        {
            case 1:
                if(World.isBlur) Blur.blurSize = animationBlur;

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
                if (World.isBlur) Blur.blurSize = animationBlur;

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
        if (World.isBlur) Blur.enabled = true;
        Time.timeScale = 0;
    }

    void disablePause()
    {
        modeAnimBlur = 2;
        Time.timeScale = 1;
    }

    void disableRevival()
    {
        btnRevival.SetActive(false);
        btnRetry.transform.localPosition = new Vector3(-290, -8, 0);
        btnHome.transform.localPosition = new Vector3( 288, -8, 0);
    }

    public void runRevival(int life = 0)
    {
        cntRevival++;
        CubeManager.life = life;
        World.isGameOver = false;
        World.isPause = false;
        World.audioVolume(1.0f);
        disablePause();
        GameOver.SetActive(false);
    }

    public void OnClick(string button)
    {
        string shareMsg;
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
            case "Revival":
                if(!AdColonyAndroid.PlayV4VCAd(this)) infoRevival.text = Msg.errRevival;
                break;
            case "Twitter":
                if(Msg.isLangJa) shareMsg = Msg.jaTwitter.Replace("{level}", SceneManager.GetActiveScene().name).Replace("{score}", World.sumScore.ToString());
                else             shareMsg = Msg.enTwitter.Replace("{level}", SceneManager.GetActiveScene().name).Replace("{score}", World.sumScore.ToString());

                Application.OpenURL(shareMsg);
                break;
            case "LINE":
                if (Msg.isLangJa) shareMsg = Msg.jaLINE.Replace("{level}", SceneManager.GetActiveScene().name).Replace("{score}", World.sumScore.ToString());
                else              shareMsg = Msg.enLINE.Replace("{level}", SceneManager.GetActiveScene().name).Replace("{score}", World.sumScore.ToString());

                Application.OpenURL(shareMsg);
                break;
            default:
                break;
        }
    }

    public void OnPressLeft(bool isPress)
    {
        isLeft = isPress;
    }

    public void OnPressRight(bool isPress)
    {
        isRight = isPress;
    }

    public void OnPressJump(bool isPress)
    {
        if (isLock)
        {
            if (!isPress) isLock = false;
            return;
        }

        if (isPress) isLock = true;

        isJump = true;
    }

    int generateRand(int rangeX = 0, int rangeY = 0)
    {
        return Random.Range(rangeX, rangeY);
    }
}
