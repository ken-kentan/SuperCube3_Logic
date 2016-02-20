using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Msg : MonoBehaviour {

    public static bool isLangJa;

    public static string sample;
    public static string appName, version, appURL, appURLenc;
    public static string jaTwitter, jaLINE,
                         enTwitter, enLINE;
    public static string[] jaHint = new string[7],
                           enHint = new string[7];
    public static string[] jaSetting = new string[2],
                           enSetting = new string[2];

    // Use this for initialization
    void Start () {
        if (Application.systemLanguage == SystemLanguage.Japanese) isLangJa = true;
        else                                                       isLangJa = false;

        sample = "Sample";

        appName   = "SuperCube3";
        version   = "dev";
        appURL    = "http://ken.kentan.jp";
        appURLenc = "http%3a%2f%2fken%2ekentan%2ejp";

        jaTwitter = "https://twitter.com/intent/tweet?text=レベル{level}をスコア{score}でクリア！ %23" + appName + " " + appURL;
        enTwitter = "https://twitter.com/intent/tweet?text=Level{level} Clear! Score{score}. %23" + appName + " " + appURL;

        jaLINE = "http://line.me/R/msg/text/?SuperCube3のレベル{level}をスコア{score}でクリアしたよ！ " + appURLenc;
        enLINE = "http://line.me/R/msg/text/?SuperCube3 Level{level}Clear! Score{score}. " + appURLenc;

        jaHint[0] = "HomeのSettingからジャイロ感度の変更が行えます。";
        jaHint[1] = "HomeのSettingでバイブレーションやパフォーマンスの変更が行えます。";
        jaHint[2] = "グリーンのブロックに触れるとセーブされます。";
        jaHint[3] = "ジャンプは連続で2回できます。";
        jaHint[4] = "RedCubeは上から踏むことで無力化できます。";
        jaHint[5] = "PinkCubeは上から踏んでも無力化できません。";
        jaHint[6] = "HomeのSettingから操作方法の変更が行えます。";

        enHint[0] = "You can change the gyro sensitivity from in the Setting of Home.";
        enHint[1] = "You can change the vibration and performance in the Setting of Home.";
        enHint[2] = "Spwan points will save when touch the block of Green.";
        enHint[3] = "Jump can be twice in a row.";
        enHint[4] = "You can incapacitate the RedCube by touch from top.";
        enHint[5] = "You cannot incapacitate the RedCube";
        jaHint[6] = "You can change the control mode from in the Setting of Home.";

        jaSetting[0] = "本体の傾きとタップで操作";
        jaSetting[1] = "画面上のボタンで操作";

        enSetting[0] = "Control by gyro and tap.";
        enSetting[1] = "Control by button in the display.";
    }
}
