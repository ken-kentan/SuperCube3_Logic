using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour {

    public Text Clear;

    private static int cntDelay;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Goal.isEnterCube)
        {
            World.pause = true;
            Time.timeScale = 0;
            Clear.enabled = true;
        }

        if (Input.GetKey(KeyCode.Escape) && cntDelay == 0 && !Goal.isEnterCube)
        {
            cntDelay = 1;
            World.pause = !World.pause;

            if (Time.timeScale != 0) Time.timeScale = 0;
            else                     Time.timeScale = 1;
        }

        cntTimer();
    }

    void cntTimer()
    {
        if (cntDelay == 0) return;

        cntDelay++;
        if (cntDelay > 20) cntDelay = 0;
    }
}
