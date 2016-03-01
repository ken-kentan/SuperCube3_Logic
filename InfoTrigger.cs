using UnityEngine;
using System.Collections;

public class InfoTrigger : MonoBehaviour {

    public GameObject thisTrigger;
    public int modeMsg;

    private static GameUIManager GameUI;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void setInstance(GameUIManager gameUI)
    {
        GameUI = gameUI;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube")
        {
            GameUI.showInfo(Msg.Info[Msg.typeLang, modeMsg]);
            World.isPause = true;
            Time.timeScale = 0;
            Destroy(thisTrigger);
        }
    }
}
