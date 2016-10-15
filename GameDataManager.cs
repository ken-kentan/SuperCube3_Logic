using UnityEngine;
using System;
using System.Collections;
using System.Text;

public class GameDataManager : MonoBehaviour {

    private static GameDataManager instance;
    private static int Score, Jump, Clear, Save;
    private static int Point, Aqua, Magnet, PlusJump;
    private static int Kill, Dead;
    private static int SecretBlock, SecretRoute;
    private static int balanceScore, expenseScore;
    public static string UUID;

    public enum Data {All=-1, Score, Jump, Clear, Save, Point, Aqua, Magnet, PlusJump, Kill, Dead, SecretBlock, SecretRoute, Balance, Expense };
    private Data type;

    public static void Reload()
    {
        instance.Awake();
    }

    // Use this for initialization
    void Awake() {
        instance = this;
        //Total
        Score = PlayerPrefs.GetInt("totalScore", 0);
        Jump  = PlayerPrefs.GetInt("totalJump",  0);
        Clear = PlayerPrefs.GetInt("totalClear", 0);
        Save  = PlayerPrefs.GetInt("totalSave",  0);

        //Collection
        Point    = PlayerPrefs.GetInt("collectPoint",    0);
        Aqua     = PlayerPrefs.GetInt("collectAqua",     0);
        Magnet   = PlayerPrefs.GetInt("collectMagnet",   0);
        PlusJump = PlayerPrefs.GetInt("collectPlusJump", 0);

        //Enemy
        Kill = PlayerPrefs.GetInt("enemyKill", 0);
        Dead = PlayerPrefs.GetInt("enemyDead", 0);

        //Secret
        SecretBlock = PlayerPrefs.GetInt("secretBlock", 0);
        SecretRoute = PlayerPrefs.GetInt("secretRoute", 0);

        //Store
        balanceScore = PlayerPrefs.GetInt("balanceScore", -1);
        expenseScore = PlayerPrefs.GetInt("expenseScore", 0);

        if (balanceScore == -1) InitWalletScore();
    }

    void InitWalletScore()
    {
        balanceScore = Score;

        PlayerPrefs.SetInt("balanceScore", balanceScore);
        PlayerPrefs.Save();

        Debug.Log("Init WalletScore: " + balanceScore);
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("totalScore", Score);
        PlayerPrefs.SetInt("totalJump", Jump);
        PlayerPrefs.SetInt("totalClear", Clear);
        PlayerPrefs.SetInt("totalSave", Save);

        PlayerPrefs.SetInt("collectPoint", Point);
        PlayerPrefs.SetInt("collectAqua", Aqua);
        PlayerPrefs.SetInt("collectMagnet", Magnet);
        PlayerPrefs.SetInt("collectPlusJump", PlusJump);

        PlayerPrefs.SetInt("enemyKill", Kill);
        PlayerPrefs.SetInt("enemyDead", Dead);

        PlayerPrefs.SetInt("secretBlock", SecretBlock);
        PlayerPrefs.SetInt("secretRoute", SecretRoute);

        PlayerPrefs.Save();

        UnityEngine.Debug.Log("Save success.");
    }

    public static string ConvertToCloudData()
    {
        System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

        foreach (Data data in Enum.GetValues(typeof(Data)))
        {
            if (data == Data.All) continue;
            
            strBuilder.Append(Get(data));
            strBuilder.Append(",");
        }

        strBuilder.Append("|");

        //最大クリアレベル
        strBuilder.Append(PlayerPrefs.GetInt("clearedMaxLevel", -1));
        strBuilder.Append(",");

        for(int i=0; i<=9; ++i)
        {
            strBuilder.Append(PlayerPrefs.GetInt("scoreHigh_" + i, -1));
            strBuilder.Append(",");
        }

        strBuilder.Append("|");

        strBuilder.Append(GetCudeLife());
        strBuilder.Append(",");
        strBuilder.Append(GetCudeJump());
        strBuilder.Append(",");
        strBuilder.Append("|");

        Debug.Log("Cloud Save data: " + strBuilder.ToString());

        return strBuilder.ToString();
    }

    public static int Get(Data typeData)
    {
        switch (typeData)
        {
            case Data.Score:
                return Score;
            case Data.Jump:
                return Jump;
            case Data.Clear:
                return Clear;
            case Data.Save:
                return Save;
            case Data.Point:
                return Point;
            case Data.Aqua:
                return Aqua;
            case Data.Magnet:
                return Magnet;
            case Data.PlusJump:
                return PlusJump;
            case Data.Dead:
                return Dead;
            case Data.Kill:
                return Kill;
            case Data.SecretBlock:
                return SecretBlock;
            case Data.SecretRoute:
                return SecretRoute;
            case Data.Expense:
                return expenseScore;
            case Data.Balance:
                return balanceScore;
            default:
                UnityEngine.Debug.LogError("Data:" + typeData + " is not exist.");
                return -1;
        }
    }

    public static int GetHighScore(string stageLevel)
    {
        return PlayerPrefs.GetInt("scoreHigh_" + stageLevel, -1);
    }

    public static int GetMaxClearedLevel()
    {
        return PlayerPrefs.GetInt("clearedMaxLevel", -1);
    }

    public static bool SetHighScore(string stageLevel, int score)
    {
        if (score > PlayerPrefs.GetInt("scoreHigh_" + stageLevel, -1))
        {
            PlayerPrefs.SetInt("scoreHigh_" + stageLevel, score);
            return true;
        }
        return false;
    }

    public static void AddDataValue(Data typeData, int value = 1)
    {
        switch (typeData)
        {
            case Data.Score:
                Score += value;
                PlayerPrefs.SetInt("balanceScore", balanceScore + value);
                break;
            case Data.Jump:
                Jump += value;
                break;
            case Data.Clear:
                Clear += value;
                break;
            case Data.Save:
                Save += value;
                break;
            case Data.Point:
                Point += value;
                break;
            case Data.Aqua:
                Aqua += value;
                break;
            case Data.Magnet:
                Magnet += value;
                break;
            case Data.PlusJump:
                PlusJump += value;
                break;
            case Data.Dead:
                Dead += value;
                break;
            case Data.Kill:
                Kill += value;
                break;
            case Data.SecretBlock:
                SecretBlock += value;
                break;
            case Data.SecretRoute:
                SecretRoute += value;
                break;
        }
        Achievements(typeData);
    }

    public static void SetMaxClearedLevel(int stageLevel)
    {
        if (stageLevel > GetMaxClearedLevel()) PlayerPrefs.SetInt("clearedMaxLevel", stageLevel);
    }

    static void Achievements(Data typeData)
    {
        if (!GPGS.isLogin) return;

        switch (typeData)
        {
            case Data.Jump://Total
                if (Jump >= 10000) GPGS.Achievements(GPGSids.achievement_aircraft);
                else if (Jump >= 1000) GPGS.Achievements(GPGSids.achievement_bird);
                else if (Jump >= 100) GPGS.Achievements(GPGSids.achievement_jumper);
                break;
            case Data.Clear:
                if (Clear >= 20) GPGS.Achievements(GPGSids.achievement_20_clear);
                else if (Clear >= 10) GPGS.Achievements(GPGSids.achievement_10_clear);
                else if (Clear >= 5) GPGS.Achievements(GPGSids.achievement_5_clear);
                break;
            case Data.Point:
                if (Point >= 10000) GPGS.Achievements(GPGSids.achievement_point_collector);
                else if (Point >= 1000) GPGS.Achievements(GPGSids.achievement_point_geek);
                else if (Point >= 500) GPGS.Achievements(GPGSids.achievement_point_love_3);
                break;
            case Data.Aqua:
                if (Aqua >= 100) GPGS.Achievements(GPGSids.achievement_1_collector);
                else if (Aqua >= 50) GPGS.Achievements(GPGSids.achievement_1_geek);
                else if (Aqua >= 10) GPGS.Achievements(GPGSids.achievement_1_love_3);
                break;
            case Data.Magnet:
                if (Magnet >= 100) GPGS.Achievements(GPGSids.achievement_magnet_monster);
                else if (Magnet >= 50) GPGS.Achievements(GPGSids.achievement_magnet_man);
                else if (Magnet >= 10) GPGS.Achievements(GPGSids.achievement_magnet_cube);
                break;
            case Data.PlusJump:
                if (PlusJump >= 100) GPGS.Achievements(GPGSids.achievement_jump_collector);
                else if (PlusJump >= 50) GPGS.Achievements(GPGSids.achievement_jump_geek);
                else if (PlusJump >= 10) GPGS.Achievements(GPGSids.achievement_jump_love_3);
                break;
            case Data.Dead:
                if (Dead >= 500) GPGS.Achievements(GPGSids.achievement_zombie);
                else if (Dead >= 100) GPGS.Achievements(GPGSids.achievement_grim_reaper);
                else if (Dead >= 50) GPGS.Achievements(GPGSids.achievement_death);
                break;
            case Data.Kill:
                if (Kill >= 100) GPGS.Achievements(GPGSids.achievement_overkill);
                else if (Kill >= 50) GPGS.Achievements(GPGSids.achievement_resistance);
                else if (Kill >= 10) GPGS.Achievements(GPGSids.achievement_killer);
                break;
            case Data.All://All
                if (Jump >= 10000) GPGS.Achievements(GPGSids.achievement_aircraft);
                if (Jump >= 1000) GPGS.Achievements(GPGSids.achievement_bird);
                if (Jump >= 100) GPGS.Achievements(GPGSids.achievement_jumper);

                if (Clear >= 20) GPGS.Achievements(GPGSids.achievement_20_clear);
                if (Clear >= 10) GPGS.Achievements(GPGSids.achievement_10_clear);
                if (Clear >= 5) GPGS.Achievements(GPGSids.achievement_5_clear);

                if (Point >= 10000) GPGS.Achievements(GPGSids.achievement_point_collector);
                if (Point >= 1000) GPGS.Achievements(GPGSids.achievement_point_geek);
                if (Point >= 500) GPGS.Achievements(GPGSids.achievement_point_love_3);

                if (Aqua >= 100) GPGS.Achievements(GPGSids.achievement_1_collector);
                if (Aqua >= 50) GPGS.Achievements(GPGSids.achievement_1_geek);
                if (Aqua >= 10) GPGS.Achievements(GPGSids.achievement_1_love_3);

                if (Magnet >= 100) GPGS.Achievements(GPGSids.achievement_magnet_monster);
                if (Magnet >= 50) GPGS.Achievements(GPGSids.achievement_magnet_man);
                if (Magnet >= 10) GPGS.Achievements(GPGSids.achievement_magnet_cube);

                if (PlusJump >= 100) GPGS.Achievements(GPGSids.achievement_jump_collector);
                if (PlusJump >= 50) GPGS.Achievements(GPGSids.achievement_jump_geek);
                if (PlusJump >= 10) GPGS.Achievements(GPGSids.achievement_jump_love_3);

                if (Dead >= 500) GPGS.Achievements(GPGSids.achievement_zombie);
                if (Dead >= 100) GPGS.Achievements(GPGSids.achievement_grim_reaper);
                if (Dead >= 50) GPGS.Achievements(GPGSids.achievement_death);

                if (Kill >= 100) GPGS.Achievements(GPGSids.achievement_overkill);
                if (Kill >= 50) GPGS.Achievements(GPGSids.achievement_resistance);
                if (Kill >= 10) GPGS.Achievements(GPGSids.achievement_killer);
                break;
            default:
                UnityEngine.Debug.Log("Data:" + typeData + " is not Achi data.");
                break;
        }
    }

    public static void CheckUUID()
    {
        if (PlayerPrefs.GetString("UUID", "None.") == "None.")//Generate
        {
            string uuid = Guid.NewGuid().ToString("D");

            PlayerPrefs.SetString("UUID", uuid);
            PlayerPrefs.Save();

            UUID = uuid;

            ServerBridge.ken_kentan_jp.UUID();
        }
        else {
            UUID = PlayerPrefs.GetString("UUID");
        }
    }

    public static int GetRetryTimes(string level)
    {
        return PlayerPrefs.GetInt("RetryLevel" + level, 0);
    }

    public static void CntRetry(string level)
    {
        PlayerPrefs.SetInt("RetryLevel" + level, 1 + GetRetryTimes(level));
        PlayerPrefs.Save();
    }

    public static int GetCudeLife()
    {
        return PlayerPrefs.GetInt("CubeLife", 3);
    }

    public static void SaveCudeLife(int value)
    {
        PlayerPrefs.SetInt("CubeLife", value);
        PlayerPrefs.Save();
    }

    public static int GetCudeJump()
    {
        return PlayerPrefs.GetInt("CubeJump", 1);
    }

    public static void SaveCudeJump(int value)
    {
        PlayerPrefs.SetInt("CubeJump", value);
        PlayerPrefs.Save();
    }

    public static int GetBalanceScore()
    {
        balanceScore = PlayerPrefs.GetInt("balanceScore", -1);
        return balanceScore;
    }

    public static bool WithdrawScore(int score)
    {
        balanceScore = PlayerPrefs.GetInt("balanceScore", -1);
        expenseScore = PlayerPrefs.GetInt("expenseScore", 0);

        if (balanceScore < score) return false;

        balanceScore -= score;
        expenseScore += score;

        PlayerPrefs.SetInt("balanceScore", balanceScore);
        PlayerPrefs.SetInt("expenseScore", expenseScore);
        PlayerPrefs.Save();

        return true;
    }
}