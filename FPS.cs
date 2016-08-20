using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS : MonoBehaviour {

    private Text textFPS;

    private int frameCount;
    private float prevTime;

    // Use this for initialization
    void Start () {
        textFPS = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        ++frameCount;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            textFPS.text = ((int)(frameCount / time)).ToString();

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }
}
