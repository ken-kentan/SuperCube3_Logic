using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUIManager : MonoBehaviour {

    public static GameUIManager thisGameUI;

    public Text Hint, Score, infoRevival, infoGPGS, cubeUnits, points, textFPS, textInfo;
    public GameObject Paused, GameOver, Clear, Controller, FPS, Info, New;
    public GameObject btnRetry, btnRevival, btnHome;
    public UnityStandardAssets.ImageEffects.BlurOptimized Blur;
    public Button btnLeft, btnRight, btnJump;

    public static bool isLeft, isRight, isJump, isInfo;
    
    private static int cntDelay, cntRevival;
    private float animationBlur;
    private int modeAnimBlur; //0:none 1:Enable 2:Disable
    private static bool isLock;

    //fps
    private int frameCount;
    private float prevTime;

    void Awake()
    {
        thisGameUI = this;
    }

    // Use this for initialization
    void Start () {
        cntDelay = cntRevival = 0;
        animationBlur = 0.0f;
        modeAnimBlur = 0;

        isLeft = isRight = isJump = false;

        //UI Init
        cubeUnits.text = "3";
        points.text = World.sumPoint.ToString();

        if (World.isController) Controller.SetActive(true);
        if (World.isDisplayFPS) FPS.SetActive(true);

        New.SetActive(false);

        //Init fps
        frameCount = 0;
        prevTime = 0.0f;

        isLock = false;

        infoGPGS.text = Msg.GPGSneedLogin[Msg.typeLang];
        infoRevival.text = Msg.Revival[Msg.typeLang];
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isDisplayFPS) updateFPS();
        if (World.isLoading || isInfo) return;

        //Main UI
        if(CubeManager.life >= 0) cubeUnits.text = CubeManager.life.ToString();
        points.text = World.sumPoint.ToString();

        //Clear
        if (Goal.isEnterCube)
        {
            World.isPause = true;
            enablePause();
            Clear.SetActive(true);
            if (World.sumScore == -1) //called at once
            {
                World.audioVolume(0.0f);
                World.calcScore();
                GameDataManager.AddDataValue(GameDataManager.Data.Score, World.sumScore);

                if (GameDataManager.SetHighScore(World.nameScene, World.sumScore)) New.SetActive(true);

                GameDataManager.SetMaxClearedLevel(int.Parse(World.nameScene));
                GameDataManager.AddDataValue(GameDataManager.Data.Clear);

                if (GPGS.isLogin)
                {
                    GPGS.Leaderboards(World.nameScene, World.sumScore);
                    GPGS.Achievements(GPGS.getAchievementsID(World.nameScene));

                    if (GameDataManager.Get(GameDataManager.Data.Clear) >= 5) GPGS.Achievements(GPGSids.achievement_5_clear);
                }

                ServerBridge.ken_kentan_jp.GameEvent("Clear");
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
                World.audioVolume(0.2f);
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
            enablePause();
            if (cntRevival >= 2 || (!AdColonyAndroid.checkV4VC() && Hint != null)) disableRevival();
            GameOver.SetActive(true);
            if (Hint != null)
            {
                Hint.text = Msg.Hint[Msg.typeLang, generateRand(0, 7)];
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

    public void SetController(bool enable = true)
    {
        if(World.isController) btnLeft.enabled = btnRight.enabled = btnJump.enabled = enable;
    }

    void enablePause()
    {
        modeAnimBlur = 1;
        if (World.isBlur) Blur.enabled = true;
        Time.timeScale = 0;
        SetController(false);
    }

    void disablePause()
    {
        modeAnimBlur = 2;
        Time.timeScale = 1;
        SetController();
    }

    void disableRevival()
    {
        btnRevival.SetActive(false);
        btnRetry.transform.localPosition = new Vector3(-290, -8, 0);
        btnHome.transform.localPosition  = new Vector3( 288, -8, 0);
    }

    void updateFPS()
    {
        ++frameCount;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            textFPS.text = ((int)(frameCount / time)).ToString();

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
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

    public void showInfo(string str)
    {
        isInfo = true;
        textInfo.text = str;
        Info.SetActive(true);
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
                if(!AdColonyAndroid.PlayV4VCAd()) infoRevival.text = Msg.errRevival;
                break;
            case "Next":
                int nextScene = int.Parse(World.nameScene) + 1;
                if (nextScene <= 8)
                {
                    World.Loading.SetActive(true);
                    Loading.RestartLoad();
                    SceneManager.LoadSceneAsync(nextScene.ToString());
                }
                else SceneManager.LoadScene("Home");
                break;
            case "Twitter":
                shareMsg = Msg.Twitter[Msg.typeLang].Replace("{level}", World.nameScene).Replace("{score}", World.sumScore.ToString());
                Application.OpenURL(shareMsg);
                break;
            case "LINE":
                shareMsg = Msg.LINE[Msg.typeLang].Replace("{level}", World.nameScene).Replace("{score}", World.sumScore.ToString());
                Application.OpenURL(shareMsg);
                break;
            case "Leaderboards":
                GPGS.showLeaderboards();
                break;
            case "Achievements":
                GPGS.showAchievements();
                break;
            case "InfoOK":
                Info.SetActive(false);
                World.isPause = false;
                Time.timeScale = 1;
                isInfo = false;
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
        if (Loading.isLoading || CubeManager.isWarpLock) return;

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
        Random.seed = System.DateTime.Now.Second;
        return Random.Range(rangeX, rangeY);
    }
}
