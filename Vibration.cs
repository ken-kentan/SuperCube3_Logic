using UnityEngine;
using System.Collections;

public class Vibration : MonoBehaviour {

    private static AndroidJavaObject unityPlayer;
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject vibrator;

    // 初期処理
    public static void Initialize()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    }

    // バイブレーション機能呼び出し
    public static void set(long msec)
    {
        vibrator.Call("vibrate", msec);
    }

    // 終了処理
    public static void Destruct()
    {
        vibrator.Dispose();
        currentActivity.Dispose();
        unityPlayer.Dispose();
    }
}
