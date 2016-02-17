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
    public static int sumPoint;
    public static bool pause;
    public static Vector3 posReborn = new Vector3(0f, 2.0f, 0f);

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

        drawDistance = 20.0f;
        sumPoint = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
