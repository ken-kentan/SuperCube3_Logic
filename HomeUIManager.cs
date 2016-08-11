using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeUIManager : MonoBehaviour
{

    public enum AnimationMode { Play, Data, Online }

    public GameObject parentLoading, btnPlay, bgPlay, btnData, bgData, btnOnline, bgOnline;
    public GameObject LevelSelect, Data, Online;
    public Text textScore, textJump, textClear, textSave, textPoint, textPlusOne, textMagnet, textPlusJump, textDead, textKill, textSBlock, textSRoute;
    public Text textUserName;
    public Text[] textHighScore;
    public Button[] btn;
    public Image[] imgBtn;
    public Image imgOnlineStatus;
    public Animator homeAnimator;
    public AudioSource audioSource;

    private bool wasPlayBGM, isReverseAnimation, fixEventBug;
    private int cntTimer;

    private static int cntSplashTimer;
    private static bool wasLaunced;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
       
        cntTimer = 0;

        imgOnlineStatus.color = GPGS.Green;

        int btnLength = btn.Length;

        for (int i = 0; i < btnLength; i++) if (GameDataManager.GetHighScore(i.ToString()) != -1) textHighScore[i].text = GameDataManager.GetHighScore(i.ToString()).ToString();

        GPGS.Login();

        setGameData();

        //Button color Init(Level Select)
        for (int i = GameDataManager.GetMaxClearedLevel() + 2; i < btnLength; i++)
        {
            btn[i].enabled = false;
            imgBtn[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (SplashScreen.WasLanched())
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GPGS.isConnecting)
        {
            if (GPGS.isLogin) imgOnlineStatus.color = GPGS.Green;
            else imgOnlineStatus.color = new Color(214.0f / 255.0f, 0, 2.0f / 255.0f, 1);
        }

        if (GPGS.isConnecting)
        {
            if (cntTimer++ < 10)
            {
                imgOnlineStatus.color = World.colorWhite;
            }
            else
            {
                imgOnlineStatus.color = World.colorAplha;
                if (cntTimer > 20) cntTimer = 0;
            }
        }
    }

    void setGameData()
    {
        textScore.text = GameDataManager.Get(GameDataManager.Data.Score).ToString();
        textJump.text = GameDataManager.Get(GameDataManager.Data.Jump).ToString();
        textClear.text = GameDataManager.Get(GameDataManager.Data.Clear).ToString();
        textSave.text = GameDataManager.Get(GameDataManager.Data.Save).ToString();

        textPoint.text = GameDataManager.Get(GameDataManager.Data.Point).ToString();
        textPlusOne.text = GameDataManager.Get(GameDataManager.Data.Aqua).ToString();
        textMagnet.text = GameDataManager.Get(GameDataManager.Data.Magnet).ToString();
        textPlusJump.text = GameDataManager.Get(GameDataManager.Data.PlusJump).ToString();

        textDead.text = GameDataManager.Get(GameDataManager.Data.Dead).ToString();
        textKill.text = GameDataManager.Get(GameDataManager.Data.Kill).ToString();

        textSBlock.text = GameDataManager.Get(GameDataManager.Data.SecretBlock).ToString();
        textSRoute.text = GameDataManager.Get(GameDataManager.Data.SecretRoute).ToString();
    }

    public void OnClick(string button)
    {
        switch (button)
        {
            case "Play":
                if (homeAnimator.GetInteger("AnimationMode") == 0)
                {
                    btnPlay.SetActive(false);
                    bgPlay.SetActive(false);
                    LevelSelect.SetActive(true);
                    homeAnimator.SetFloat("Speed", 1);
                    homeAnimator.SetInteger("AnimationMode", 1);
                }
                break;
            case "Back":
                isReverseAnimation = fixEventBug = true;
                homeAnimator.SetFloat("Speed", -1);
                break;
            case "Data":
                if (homeAnimator.GetInteger("AnimationMode") == 0)
                {
                    Data.SetActive(true);
                    btnData.SetActive(false);
                    bgData.SetActive(false);
                    homeAnimator.SetFloat("Speed", 1);
                    homeAnimator.SetInteger("AnimationMode", 2);
                }
                break;
            case "Online":
                if (homeAnimator.GetInteger("AnimationMode") == 0)
                {
                    if (!GPGS.isLogin)
                    {
                        GPGS.Login();
                        return;
                    }
                    Online.SetActive(true);
                    btnOnline.SetActive(false);
                    bgOnline.SetActive(false);
                    homeAnimator.SetFloat("Speed", 1);
                    homeAnimator.SetInteger("AnimationMode", 3);
                    textUserName.text = GPGS.userName;
                }
                break;
            case "Setting":
                SceneManager.LoadScene("Setting");
                break;
            case "Feedback":
                Application.OpenURL("https://play.google.com/store/apps/details?id=jp.kentan.supercubeworld");
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

        Debug.Log(button + " Click.");
    }

    public void OnClickLevel(string level)
    {
        parentLoading.SetActive(true);
        SceneManager.LoadSceneAsync(level);
    }

    public void StopAnimation()
    {
        homeAnimator.SetFloat("Speed", 0);
        homeAnimator.SetTime(1.0D);
        Debug.Log("Animation stop.");
    }

    public void PlayAnimation(AnimationMode mode)
    {
        if (fixEventBug || !isReverseAnimation)
        {
            fixEventBug = false;
            return;
        }

        switch (mode)
        {
            case AnimationMode.Play:
                btnPlay.SetActive(true);
                bgPlay.SetActive(true);
                LevelSelect.SetActive(false);
                break;
            case AnimationMode.Data:
                btnData.SetActive(true);
                bgData.SetActive(true);
                Data.SetActive(false);
                break;
            case AnimationMode.Online:
                btnOnline.SetActive(true);
                bgOnline.SetActive(true);
                Online.SetActive(false);
                break;
        }

        homeAnimator.SetFloat("Speed", 0);
        homeAnimator.SetTime(0.0D);
        homeAnimator.SetInteger("AnimationMode", 0);
        isReverseAnimation = false;

        Debug.Log(mode + " Start.");
    }
}
