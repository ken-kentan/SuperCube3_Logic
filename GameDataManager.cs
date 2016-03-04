using UnityEngine;
using System.Collections;

public class GameDataManager : MonoBehaviour {

    public static int Score, Jump, Clear, Save;
    public static int Point, Aqua, Magnet;
    public static int Kill, Dead;
    public static int SecretBlock, SecretRoute;

	// Use this for initialization
	void Start () {
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
	void Update () {
	}

    public static void SaveTotal()
    {
        PlayerPrefs.SetInt("totalScore", Score);
        PlayerPrefs.SetInt("totalJump",   Jump);
        PlayerPrefs.SetInt("totalClear", Clear);
        PlayerPrefs.SetInt("totalSave",   Save);
        PlayerPrefs.Save();
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
}
