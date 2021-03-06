﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    public static GameObject Cube, EnemyChieldren, EnemyPatricle, EnemyTracking, EnemyMove, EnemyStaticMove, Magnet, Aqua, PlusJump,  BigPoint, Point, Loading, SpringBlock, Dialog;
    public static AudioSource audioSource;
    public static AudioClip killEnemySE, getAquaSE, getMagnetSE, pointSE, saveSE, jumpSE, contactSE, damageSE, getJumpSE, dropEnemySE, findItemSE;
    public static Color alpha = new Color(0, 0, 0, 0.01f);
    public static Material materialAqua, materialMagnet, materialBlockSecret, materialPlusJump, materialBlack, materialCube;
    public static float drawDistance;
    public static Color colorAqua   = new Color(0.0f, 0.5f, 1.0f, 1.0f),
                        colorMagnet = new Color(0.6f, 1.0f, 0.0f, 1.0f),
                        colorWhite  = new Color(222.0f / 255.0f, 236.0f / 255.0f, 1.0f, 1.0f),
                        colorAplha  = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public static bool isPause, isGameOver, isClear, isLoading;
    public static Vector3 posReborn;
    public static string nameScene;

    public static CubeManager cubeManager;
    public static CubeEffects effect;
    public static List<LiftManager> liftManagerList = new List<LiftManager>();
    public static List<FloorVanish> floorVanishList = new List<FloorVanish>();
    public static List<EnemyManager> enemyManagerList = new List<EnemyManager>();
    public static List<EnemyChildren> enemyChildrenList = new List<EnemyChildren>();

    //Game data
    public static int sumPoint, sumJump, sumDead, sumKill, sumScore, sumAqua, sumMagnet;

    //Setting data
    public static bool isController, isVibration, isBlur, isDisplayFPS;

    private static bool isChangeVolume;
    private static float volume, targetVolume;

    // Use this for initialization
    void Awake () {
        Application.targetFrameRate = 60;

        cubeManager = null;
        effect = null;

        liftManagerList.Clear();
        floorVanishList.Clear();
        enemyManagerList.Clear();
        enemyChildrenList.Clear();

        Cube    = GameObject.Find("Cube");
        Loading = GameObject.Find("Loading");
        audioSource = Cube.GetComponent<AudioSource>();

        EnemyChieldren  = Resources.Load("Objects/EnemyChieldren")  as GameObject;
        EnemyPatricle   = Resources.Load("Objects/EnemyParticle")   as GameObject;
        Magnet          = Resources.Load("Objects/Magnet")          as GameObject;
        Aqua            = Resources.Load("Objects/Aqua")            as GameObject;
        PlusJump        = Resources.Load("Objects/PlusJump")        as GameObject;
        BigPoint        = Resources.Load("Objects/BigPoint")        as GameObject;
        Point           = Resources.Load("Objects/Point")           as GameObject;
        EnemyTracking   = Resources.Load("Objects/EnemyTracking")   as GameObject;
        EnemyMove       = Resources.Load("Objects/EnemyMove")       as GameObject;
        EnemyStaticMove = Resources.Load("Objects/EnemyStaticMove") as GameObject;
        SpringBlock     = Resources.Load("Objects/SpringBlock")     as GameObject;

        Dialog = Resources.Load("Objects/Dialog") as GameObject;

        killEnemySE = Resources.Load("SEs/kill_enemy") as AudioClip;
        getAquaSE   = Resources.Load("SEs/aqua_get")   as AudioClip;
        getMagnetSE = Resources.Load("SEs/magnet_get") as AudioClip;
        pointSE     = Resources.Load("SEs/1point_get") as AudioClip;
        saveSE      = Resources.Load("SEs/save")       as AudioClip;
        jumpSE      = Resources.Load("SEs/jump")       as AudioClip;
        contactSE   = Resources.Load("SEs/contact")    as AudioClip;
        damageSE    = Resources.Load("SEs/damage")     as AudioClip;
        getJumpSE   = Resources.Load("SEs/plus_jump")  as AudioClip;
        dropEnemySE = Resources.Load("SEs/drop_enemy") as AudioClip;
        findItemSE  = Resources.Load("SEs/find_item")  as AudioClip;

        materialAqua        = Resources.Load("Materials/m_aqua")        as Material;
        materialMagnet      = Resources.Load("Materials/m_magnet")      as Material;
        materialBlockSecret = Resources.Load("Materials/m_blocksecret") as Material;
        materialPlusJump    = Resources.Load("Materials/m_jump")        as Material;
        materialBlack       = Resources.Load("Materials/m_black")       as Material;
        materialCube        = Resources.Load("Materials/m_cube")        as Material;

        //Setting data
        isController = (PlayerPrefs.GetInt("isController", 1) != 0);
        isVibration  = (PlayerPrefs.GetInt("isVibration" , 1) != 0);
        isBlur       = (PlayerPrefs.GetInt("isBlur"      , 1) != 0);
        isDisplayFPS = (PlayerPrefs.GetInt("isDisplayFPS", 0) != 0);

        if (PlayerPrefs.GetInt("isHighQuality", 1) == 1)
        {
            QualitySettings.SetQualityLevel(2, true);
        }else
        {
            QualitySettings.SetQualityLevel(0, true);
        }

            drawDistance = 20.0f;

        //sum Init
        sumPoint = sumJump = sumDead = sumKill = sumAqua = sumMagnet = 0;
        sumScore = -1;

        //UI Init
        isLoading = true;
        isPause = isGameOver = isClear = false;

        isChangeVolume = false;
        volume = 1.0f;

        posReborn = new Vector3(0f, 2.0f, 0f);

        nameScene = SceneManager.GetActiveScene().name;

        Time.timeScale = 0;
        isPause = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (isChangeVolume) ChangeVolume();
    }

    public static void SetAudioVolume(float volume)
    {
        targetVolume = volume;
        isChangeVolume = true;
    }

    static void ChangeVolume()
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
        sumScore = sumPoint*10 + cubeManager.life*100 + sumJump + sumKill*5 + sumAqua*5 + sumMagnet*5 - sumDead;
    }
}
