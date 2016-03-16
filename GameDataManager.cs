using UnityEngine;
using System;
using System.Collections;

public class GameDataManager : MonoBehaviour {

    public static int Score, Jump, Clear, Save;
    public static int Point, Aqua, Magnet;
    public static int Kill, Dead;
    public static int SecretBlock, SecretRoute;
    public static string UUID, UUIDinfo;

	// Use this for initialization
	void Awake() {
        //Total
        Score = PlayerPrefs.GetInt("totalScore", 0);
        Jump  = PlayerPrefs.GetInt("totalJump",  0);
        Clear = PlayerPrefs.GetInt("totalClear", 0);
        Save  = PlayerPrefs.GetInt("totalSave",  0);

        //Collection
        Point  = PlayerPrefs.GetInt("collectPoint",  0);
        Aqua   = PlayerPrefs.GetInt("collectAqua",   0);
        Magnet = PlayerPrefs.GetInt("collectMagnet", 0);

        //Enemy
        Kill = PlayerPrefs.GetInt("enemyKill", 0);
        Dead = PlayerPrefs.GetInt("enemyDead", 0);

        //Secret
        SecretBlock = PlayerPrefs.GetInt("secretBlock", 0);
        SecretRoute = PlayerPrefs.GetInt("secretRoute", 0);
    }

    // Update is called once per frame
    void Update() {
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

    public static void SetMaxClearedLevel(int stageLevel)
    {
        if (stageLevel > GetMaxClearedLevel()) PlayerPrefs.SetInt("clearedMaxLevel", stageLevel);
    }

    public static void SaveTotal()
    {
        PlayerPrefs.SetInt("totalScore", Score);
        PlayerPrefs.SetInt("totalJump",   Jump);
        PlayerPrefs.SetInt("totalClear", Clear);
        PlayerPrefs.SetInt("totalSave",   Save);
        PlayerPrefs.Save();

        Achievement(0);
    }

    public static void SaveCollection()
    {
        PlayerPrefs.SetInt("collectPoint",   Point);
        PlayerPrefs.SetInt("collectAqua",     Aqua);
        PlayerPrefs.SetInt("collectMagnet", Magnet);
        PlayerPrefs.Save();
    }

    public static void SaveEnemy()
    {
        PlayerPrefs.SetInt("enemyKill", Kill);
        PlayerPrefs.SetInt("enemyDead", Dead);
        PlayerPrefs.Save();
    }

    public static void SaveSecret()
    {
        PlayerPrefs.SetInt("secretBlock", SecretBlock);
        PlayerPrefs.SetInt("secretRoute", SecretRoute);
        PlayerPrefs.Save();
    }

    public static void Achievement(int type)
    {
        if (!GPGS.isLogin) return;

        switch (type)
        {
            case 0://Total
                if (Jump >= 10000) GPGS.Achievements(GPGSids.achievement_aircraft);
                else if (Jump >= 1000) GPGS.Achievements(GPGSids.achievement_bird);
                else if (Jump >= 100) GPGS.Achievements(GPGSids.achievement_jumper);

                if (Clear >= 20) GPGS.Achievements(GPGSids.achievement_20_clear);
                else if (Clear >= 10) GPGS.Achievements(GPGSids.achievement_10_clear);
                else if (Clear >= 5) GPGS.Achievements(GPGSids.achievement_5_clear);
                break;
            case 1://Collection
                if (Point >= 10000) GPGS.Achievements(GPGSids.achievement_point_collector);
                else if (Point >= 1000) GPGS.Achievements(GPGSids.achievement_point_geek);
                else if (Point >= 500) GPGS.Achievements(GPGSids.achievement_point_love_3);

                if (Aqua >= 100) GPGS.Achievements(GPGSids.achievement_1_collector);
                else if (Aqua >= 50) GPGS.Achievements(GPGSids.achievement_1_geek);
                else if (Aqua >= 10) GPGS.Achievements(GPGSids.achievement_1_love_3);

                if (Magnet >= 100) GPGS.Achievements(GPGSids.achievement_magnet_monster);
                else if (Magnet >= 50) GPGS.Achievements(GPGSids.achievement_magnet_man);
                else if (Magnet >= 10) GPGS.Achievements(GPGSids.achievement_magnet_cube);
                break;
            case -1://All
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
                else if (Aqua >= 50) GPGS.Achievements(GPGSids.achievement_1_geek);
                else if (Aqua >= 10) GPGS.Achievements(GPGSids.achievement_1_love_3);

                if (Magnet >= 100) GPGS.Achievements(GPGSids.achievement_magnet_monster);
                else if (Magnet >= 50) GPGS.Achievements(GPGSids.achievement_magnet_man);
                else if (Magnet >= 10) GPGS.Achievements(GPGSids.achievement_magnet_cube);
                break;
        }
    }

    public static void CheckUUID()
    {
        if (PlayerPrefs.GetString("UUID", "None.") == "None.")//Generate
        {
            PlayerPrefs.SetString("UUID_info", SystemInfo.operatingSystem + "," + SystemInfo.deviceModel + "," + DateTime.Now + "," + Msg.appVer);
            PlayerPrefs.SetString("UUID", Guid.NewGuid().ToString("D"));
            PlayerPrefs.Save();
        }

        UUIDinfo = PlayerPrefs.GetString("UUID_info");
        UUID = PlayerPrefs.GetString("UUID");

        ServerBridge.ken_kentan_jp.SendUUIDinfo();
    }
}
