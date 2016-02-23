using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeUIManager : MonoBehaviour {

    public GameObject parentLoading;

	// Use this for initialization
	void Start () {
        GPGS.Login();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick(string button)
    {
        switch (button)
        {
            case "Play":
                parentLoading.SetActive(true);
                SceneManager.LoadSceneAsync("Beta0");
                break;
            case "Setting":
                SceneManager.LoadScene("Setting");
                break;
            case "Exit":
                Application.Quit();
                break;
            case "GPGS":
                GPGS.Login();
                break;
        }
    }
}
