﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Debug : MonoBehaviour {

    public Text FPS, StatusCube;

    private int frameCount;
    private float prevTime;

    // Use this for initialization
    void Start () {
        frameCount = 0;
        prevTime = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        ++frameCount;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            FPS.text = (int)(frameCount / time) + " fps";

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }

        StatusCube.text = "Pos(X,Y) = " + (int)CubeManager.posX + "," + (int)CubeManager.posY + "\nSpeed(X,Y) = " + (int)CubeManager.speedX + "," + (int)CubeManager.speedY +
            "\nLife = " + CubeManager.life + "\nmodeEffect = " + CubeManager.effectAqua + "," + CubeManager.effectMagnet;
    }
}
