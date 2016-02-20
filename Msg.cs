using UnityEngine;
using System.Collections;

public class Msg : MonoBehaviour {

    public static bool isLangJa;

    public static string sample;
    public static string[] jaHint = new string[6],
                           enHint = new string[6];

    // Use this for initialization
    void Start () {
        if (Application.systemLanguage == SystemLanguage.Japanese) isLangJa = true;
        else                                                       isLangJa = false;

        sample = "Sample";

        jaHint[0] = "HomeのSettingからジャイロ感度を変更できます。";
        jaHint[1] = "HomeのSettingでバイブレーションやパフォーマンスの変更ができます。";
        jaHint[2] = "グリーンのブロックに触れるとセーブされます。";
        jaHint[3] = "ジャンプは連続で2回できます。";
        jaHint[4] = "RedCubeは上から踏むことで無力化できます。";
        jaHint[5] = "PinkCubeは上から踏んでも無力化できません。";

        enHint[0] = "You can change the gyro sensitivity from in the Setting of Home.";
        enHint[1] = "You can change the vibration and performance in the Setting of Home.";
        enHint[2] = "Spwan points will save when touch the block of Green.";
        enHint[3] = "Jump can be twice in a row.";
        enHint[4] = "You can incapacitate the RedCube by touch from top.";
        enHint[5] = "You cannot incapacitate the RedCube";
    }
}
