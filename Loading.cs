using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

    public GameObject thisParent, imgLoading;
    public bool isHome;

    private int frameCount, frameSum;
    private float prevTime, fps;

    private static float angle;

    // Use this for initialization
    void Start () {
        frameCount = 0;
        prevTime = fps = 0.0f;

        if(angle != default(float)) imgLoading.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
	
	// Update is called once per frame
	void Update () {
        if (isHome)
        {
            imgLoading.transform.Rotate(0, 0, -1.5f);
            return;
        }

        if (fps >= 60.0f || frameSum > 300) finishLoad();

        ++frameCount;
        ++frameSum;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            fps = frameCount / time;

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }

        angle -= 1.5f;

        imgLoading.transform.Rotate(0, 0, -1.5f);
    }

    void finishLoad()
    {
        Time.timeScale = 1;
        World.isLoading = false;
        World.isPause   = false;
        Destroy(thisParent);
    }
}
