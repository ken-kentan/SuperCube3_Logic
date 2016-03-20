using UnityEngine;
using System.Collections;

public class InfoTrigger : MonoBehaviour {
    
    public int modeMsg;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube")
        {
            GameUIManager.thisGameUI.showInfo(Msg.Info[Msg.typeLang, modeMsg]);
            World.isPause = true;
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }
}
