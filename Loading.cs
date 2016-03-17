using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loading : MonoBehaviour {

    public GameObject thisParent, imgLoading;
    public static bool isHome;

    private static int frameCount, frameSum;
    private float prevTime, fps;

    private static GameObject staticImgLoading;
    private static float angle;
    private static bool isLoading;

    // Use this for initialization
    void Start () {
        frameCount = frameSum = 0;
        prevTime = fps = 0.0f;

        staticImgLoading = imgLoading;

        if (SceneManager.GetActiveScene().name == "Home") isHome = true;
        else                                              isHome = false;

        if (isHome) angle = default(float);
        else imgLoading.transform.rotation = Quaternion.Euler(0, 0, angle);

        isLoading = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLoading) return;

        if (isHome)
        {
            angle -= 1.5f;
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

        imgLoading.transform.Rotate(0, 0, -1.5f);
    }

    void finishLoad()
    {
        Time.timeScale = 1;
        World.isLoading = false;
        World.isPause   = false;
        isLoading = false;
        thisParent.SetActive(false);
        GameUIManager.thisGameUI.SetController();
    }

    public static void RestartLoad()
    {
        isLoading = isHome = true;
        Time.timeScale = 0;
        World.isLoading = true;
        World.isPause = true;

        angle = default(float);
        staticImgLoading.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
