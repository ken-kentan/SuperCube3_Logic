using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeUIManager : MonoBehaviour {

    public GameObject parentLoading, btnPlay, bgPlay, btnData, bgData, btnOnline, bgOnline;
    public GameObject LevelSelect, Data, Online;
    public Text textOnlineStatus;
    public Text textScore, textJump, textClear, textSave, textPoint, textPlusOne, textMagnet, textDead, textKill, textSBlock, textSRoute;
    public Text textUserName;
    public Text[] textHighScore = new Text[6];
    public Button[] btn = new Button[6];
    public Image[] imgBtn = new Image[6];
    public Animator thisAnimator;

    private bool isFirst;
    private int cntTimer;

    // Use this for initialization
    void Start() {
        Time.timeScale = 1;

        isFirst = true;
        cntTimer = 0;

        thisAnimator.SetFloat("Speed", 0);

        textOnlineStatus.color = GPGS.Green;

        for(int i = 0; i < 6; i++) if (GameDataManager.GetHighScore(i.ToString()) != -1) textHighScore[i].text = GameDataManager.GetHighScore(i.ToString()).ToString();

        GPGS.Login();
        
        setGameData();

        //Button color Init(Level Select)
        for (int i = GameDataManager.GetMaxClearedLevel() + 2; i < 6; i++)
        {
            btn[i].enabled = false;
            imgBtn[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GPGS.isConnecting)
        {
            textOnlineStatus.text = "●";
            if (GPGS.isLogin) textOnlineStatus.color = GPGS.Green;
            else              textOnlineStatus.color = new Color(214.0f / 255.0f, 0, 2.0f / 255.0f, 1);
        }

        if (GPGS.isConnecting)
        {
            textOnlineStatus.color = World.colorWhite;

            if (cntTimer++ < 10)
            {
                textOnlineStatus.text = "●";
            }
            else {
                textOnlineStatus.text = "";
                if (cntTimer > 20) cntTimer = 0;
            }
        }
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
                thisAnimator.Play("openLevelSelect");
                thisAnimator.SetFloat("Speed", 1);
                break;
            case "Back":
                thisAnimator.SetFloat("Speed", -1);
                break;
            case "Data":
                isFirst = true;
                Data.SetActive(true);
                btnData.SetActive(false);
                bgData.SetActive(false);
                thisAnimator.Play("openGameData");
                thisAnimator.SetFloat("Speed", 1);
                break;
            case "Online":
                if (!GPGS.isLogin)
                {
                    GPGS.Login();
                    return;
                }
                isFirst = true;
                Online.SetActive(true);
                btnOnline.SetActive(false);
                bgOnline.SetActive(false);
                thisAnimator.Play("openOnlineService");
                thisAnimator.SetFloat("Speed", 1);
                textUserName.text = GPGS.userName;
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
            case "Achievements":
                GPGS.showAchievements();
                break;
            case "Leaderboards":
                GPGS.showLeaderboards();
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
        thisAnimator.SetFloat("Speed", 0);
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
                    thisAnimator.SetFloat("Speed", 0);
                    LevelSelect.SetActive(false);
                    UnityEngine.Debug.Log("LS Start.");
                }
                break;
            case "GameData":
                if (!btnData.activeInHierarchy)
                {
                    btnData.SetActive(true);
                    bgData.SetActive(true);
                    thisAnimator.SetFloat("Speed", 0);
                    Data.SetActive(false);
                    UnityEngine.Debug.Log("GD Start.");
                }
                break;
            case "OnlineService":
                if (!btnOnline.activeInHierarchy)
                {
                    btnOnline.SetActive(true);
                    bgOnline.SetActive(true);
                    thisAnimator.SetFloat("Speed", 0);
                    Online.SetActive(false);
                    UnityEngine.Debug.Log("OS Start.");
                }
                break;
        }
        UnityEngine.Debug.Log("Start.");
    }
}
