using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    private static readonly int SPLASH_TIME = 300;

    public GameObject objSplash;
    public Image imgSplash, imgParent;
    public Text textSplash;
    public AudioSource audioSource;

    private int cntSplashTimer = 0;

    private static bool wasLanched;

	// Use this for initialization
	void Awake () {
        if (!wasLanched)
        {
            objSplash.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(cntSplashTimer > SPLASH_TIME)
        {
            if (!wasLanched)
            {
                audioSource.Play();
                wasLanched = true;
            }
            objSplash.SetActive(false);

            return;
        }

        EffectFadein();
        EffectFadeout();

        if (GPGS.isConnecting)
        {
            textSplash.text = "Connecting to Google Play Game Services.";
        }
        else if (ServerBridge.isConnecting)
        {
            textSplash.text = "Connecting to kentan.jp.";
        }
        else
        {
            if (GPGS.isLogin) textSplash.text = "Login success.";
            else textSplash.text = "Login failed...";
        }

        ++cntSplashTimer;
    }

    void EffectFadein()
    {
        Color color = imgSplash.color;
        color.a = cntSplashTimer / 50.0f;
        imgSplash.color = color;
    }

    void EffectFadeout()
    {
        if (cntSplashTimer < (SPLASH_TIME - 15)) return;

        Color color = imgParent.color;
        color.a = (SPLASH_TIME - cntSplashTimer) / 15.0f;
        imgParent.color = imgSplash.color = color;
    }

    public static bool WasLanched()
    {
        return wasLanched;
    }
}
