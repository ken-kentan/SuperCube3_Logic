using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreUIManager : MonoBehaviour {

    public GameObject panel, objClose;

    public Animator animator;

    public Text textBalanceScore, textClose;

    public Text[] textInfo = new Text[2];

    public Image[] imgLife = new Image[2];
    public Image[] imgJump = new Image[4];
    public Button[] btnUpgrade = new Button[2];
    public Text[] textUpgrade = new Text[2];

    private static Color White = new Color(221.0f / 255.0f, 236.0f / 255.0f, 1.0f, 1.0f);
    private static Color WhiteAlpha = new Color(221.0f / 255.0f, 236.0f / 255.0f, 1.0f, 0.2f);
    private static Color WhiteBlue = new Color(120.0f / 255.0f, 174.0f / 255.0f, 246.0f / 255.0f, 1.0f);
    private static Color Green = new Color(206.0f / 255.0f, 1.0f, 0.0f, 1.0f);
    private static Color GreenAlpha = new Color(206.0f / 255.0f, 1.0f, 0.0f, 0.2f);
    private static Color Red = new Color(214.0f / 255.0f, 0.0f, 2.0f / 255.0f, 1.0f);
    private static Color Yellow = new Color(1.0f, 1.0f, 0.0f, 1.0f);

    public GameObject Controller;

    public int life, jump;

    //Animation
    private int[] cntTimerCost = new int[2];
    private bool[] isEnabledCost = new bool[2];
    private bool[] isMaxUpgrade = new bool[2];
    private bool isHide;
    private float posHideEffect;

    //Cost
    private static readonly int[] COST_UPGRADE_LIFE = { 1000, 10000 };
    private static readonly int[] COST_UPGRADE_JUMP = { 500, 1000, 5000, 10000 };

    private bool isLock;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;

        life = GameDataManager.GetCudeLife();
        jump = GameDataManager.GetCudeJump();

        SetCubeLife(life);
        SetCubeJump(jump);

        textBalanceScore.text = GameDataManager.GetBalanceScore().ToString();

        textInfo[0].text = Msg.Store[Msg.typeLang, 0];
        textInfo[1].text = Msg.Store[Msg.typeLang, 1];

        if (World.isController) Controller.SetActive(true);

        World.isPause = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isMaxUpgrade[0] && ++cntTimerCost[0] > 120)
        {
            cntTimerCost[0] = 0;
            isEnabledCost[0] = !isEnabledCost[0];

            if (isEnabledCost[0]) DisplayCostButton(0, COST_UPGRADE_LIFE[life - 3].ToString());
            else DisplayUpgradeButton(0);
        }

        if (!isMaxUpgrade[1] && ++cntTimerCost[1] > 120)
        {
            cntTimerCost[1] = 0;
            isEnabledCost[1] = !isEnabledCost[1];

            if (isEnabledCost[1]) DisplayCostButton(1, COST_UPGRADE_JUMP[jump - 1].ToString());
            else DisplayUpgradeButton(1);
        }
    }

    void SetCubeLife(int value)
    {
        imgLife[0].color = WhiteAlpha;
        imgLife[1].color = WhiteAlpha;

        if (value >= 4) imgLife[0].color = White;
        if (value >= 5)
        {
            isMaxUpgrade[0] = true;
            imgLife[1].color = White;
            DisableUpgradeButton(0);
            return;
        }

        if (isEnabledCost[0]) DisplayCostButton(0, COST_UPGRADE_LIFE[life - 3].ToString());
        else DisplayUpgradeButton(0);
    }

    void SetCubeJump(int value)
    {
        switch (value)
        {
            case 1:
                World.cubeManager.jumpPower = 300.0f;
                break;
            case 2:
                World.cubeManager.jumpPower = 325.0f;
                break;
            case 3:
                World.cubeManager.jumpPower = 350.0f;
                break;
            case 4:
                World.cubeManager.jumpPower = 375.0f;
                break;
            case 5:
                World.cubeManager.jumpPower = 400.0f;
                break;
        }

        for (int i = 0; i < 4; ++i)
        {
            imgJump[i].color = GreenAlpha;
        }

        if (value >= 2) imgJump[0].color = Green;
        if (value >= 3) imgJump[1].color = Green;
        if (value >= 4) imgJump[2].color = Green;
        if (value >= 5)
        {
            isMaxUpgrade[1] = true;
            imgJump[3].color = Green;
            DisableUpgradeButton(1);
            return;
        }

        if (isEnabledCost[1]) DisplayCostButton(1, COST_UPGRADE_JUMP[jump - 1].ToString());
        else DisplayUpgradeButton(1);
    }

    bool Withdraw(int score)
    {
        if (GameDataManager.WithdrawScore(score))
        {
            World.audioSource.PlayOneShot(World.saveSE);
            textBalanceScore.text = GameDataManager.GetBalanceScore().ToString();
            return true;
        }
        return false;
    }

    void UpgradeCubeLife()
    {
        if (life >= 5)
        {
            DisableUpgradeButton(0);
            return;
        }

        if (Withdraw(COST_UPGRADE_LIFE[life - 3]))
        {
            ++life;
            SetCubeLife(life);
            GameDataManager.SaveCudeLife(life);
        }
    }

    void UpgradeCubeJump()
    {
        if (life >= 5)
        {
            DisableUpgradeButton(1);
            return;
        }

        if (Withdraw(COST_UPGRADE_JUMP[jump - 1]))
        {
            ++jump;
            SetCubeJump(jump);
            GameDataManager.SaveCudeJump(jump);
        }
    }

    public void OnClickUpgrade(string type)
    {
        switch (type)
        {
            case "Life":
                UpgradeCubeLife();
                break;
            case "Jump":
                UpgradeCubeJump();
                break;
            default:
                Debug.LogError("Unknown button clicked.");
                break;
        }
    }

    public void OnClickExit()
    {
        SceneManager.LoadScene("Home");
    }

    void DisableUpgradeButton(int index)
    {
        ColorBlock cb = btnUpgrade[index].colors;
        cb.pressedColor = Red;
        btnUpgrade[index].colors = cb;
        textUpgrade[index].text = "M A X";
        textUpgrade[index].color = Red;
    }

    void DisplayCostButton(int index, string cost)
    {
        ColorBlock cb = btnUpgrade[index].colors;
        cb.pressedColor = Yellow;
        btnUpgrade[index].colors = cb;

        textUpgrade[index].color = Yellow;
        textUpgrade[index].text = cost;
    }

    void DisplayUpgradeButton(int index)
    {
        ColorBlock cb = btnUpgrade[index].colors;
        cb.pressedColor = WhiteBlue;
        btnUpgrade[index].colors = cb;

        textUpgrade[index].color = WhiteBlue;
        textUpgrade[index].text = "Upgrade";
    }

    public void OnPressLeft(bool isPress)
    {
        GameUIManager.isLeft = isPress;
    }

    public void OnPressRight(bool isPress)
    {
        GameUIManager.isRight = isPress;
    }

    public void OnPressJump(bool isPress)
    {
        if (World.cubeManager.isWarpLock) return;

        if (isLock)
        {
            if (!isPress) isLock = false;
            return;
        }

        if (isPress) isLock = true;

        GameUIManager.isJump = true;
    }

    public void OnClickHide()
    {
        isHide = !isHide;

        if (isHide) animator.SetInteger("AnimationMode", 1);
        else animator.SetInteger("AnimationMode", 0);
    }

    public void SetCloseButtonText(string text)
    {
        textClose.text = text;
    }
}
