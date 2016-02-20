using UnityEngine;
using System;
using System.Collections;

public class World : MonoBehaviour {

    public static GameObject Cube, EnemyChieldren;
    public static AudioSource audioSource;
    public static AudioClip killEnemySE, getAquaSE, getMagnetSE, pointSE, saveSE;
    public static Color alpha = new Color(0, 0, 0, 0.01f);
    public static Material materialAqua, materialMagnet;
    public static float drawDistance;
    public static Color colorAqua   = new Color(0, 0.5f, 1, 1),
                        colorMagnet = new Color(0.6f, 1, 0, 1);
    public static bool isPause, isGameOver, isClear;
    public static Vector3 posReborn;

    //Game data
    public static int sumPoint, sumJump, sumDead, sumKill, sumScore, sumAqua, sumMagnet;

    //Setting data
    public static bool isController, isVibration, isBlur;

    private static bool isChangeVolume;
    private static float volume, targetVolume;

    // Use this for initialization
    void Start () {
        Cube = GameObject.Find("Cube");
        audioSource = Cube.GetComponent<AudioSource>();

        EnemyChieldren = Resources.Load("Objects/EnemyChieldren") as GameObject;

        killEnemySE = Resources.Load("SEs/kill_enemy") as AudioClip;
        getAquaSE   = Resources.Load("SEs/aqua_get")   as AudioClip;
        getMagnetSE = Resources.Load("SEs/magnet_get") as AudioClip;
        pointSE     = Resources.Load("SEs/1point_get") as AudioClip;
        saveSE      = Resources.Load("SEs/save")       as AudioClip;

        //Setting data
        isController = (PlayerPrefs.GetInt("isController", 0) != 0);
        isVibration  = (PlayerPrefs.GetInt("isVibration" , 1) != 0);
        isBlur       = (PlayerPrefs.GetInt("isBlur"      , 1) != 0);

        drawDistance = 20.0f;
        sumPoint = 0;

        //UI Init
        isPause = isGameOver = isClear = false;

        isChangeVolume = false;
        volume = 1.0f;

        posReborn = new Vector3(0f, 2.0f, 0f);

        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (isChangeVolume) changeVolume();
    }

    public static void audioVolume(float _volume)
    {
        isChangeVolume = true;
        targetVolume = _volume;
    }

    static void changeVolume()
    {
        float diff = Mathf.Abs(volume - targetVolume);
        if (-0.01f < diff && diff < 0.01f)
        {
            isChangeVolume = false;
            audioSource.volume = volume = targetVolume;
        }
        volume += (targetVolume - volume) / 7.0f;
        volume = Mathf.Min(Mathf.Max(volume, 0.0f), 1.0f);
        audioSource.volume = volume;
    }

    public static void calcScore()
    {
        UnityEngine.Debug.Log(sumPoint + "," + CubeManager.life);
        sumScore = sumPoint*10 + CubeManager.life*100 + sumJump + sumKill*5 + sumAqua*5 + sumMagnet*5 - sumKill*10;
    }
}
