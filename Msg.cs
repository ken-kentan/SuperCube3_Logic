using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Msg : MonoBehaviour {
    
    public static int typeLang;

    public static string sample;
    public const string appName   = "SuperCube_World",
                        appVer    = "0.3.5beta",
                        appURL    = "http://ken.kentan.jp/SuperCube_World",
                        appURLenc = "http%3a%2f%2fken%2ekentan%2ejp%2fSuperCube_World";
    public static string[] Twitter = new string[2],
                           LINE    = new string[2],
                           Revival = new string[2];
    public static string errRevival, failRevival;
    public static string[,] Info = new string[2, 6];
    public static string[,] Hint = new string[2, 7];
    public static string[,] Setting = new string[2, 2];
    public static string[] GPGSneedLogin    = new string[2],
                           GPGSsendingScore = new string[2],
                           GPGSsendScore    = new string[2],
                           GPGSfaildScore   = new string[2];

    // Use this for initialization
    void Start () {
        int ja = 0, en = 1;

        if (Application.systemLanguage == SystemLanguage.Japanese) typeLang = ja;
        else                                                       typeLang = en;

        sample = "Sample";

        Twitter[ja] = "https://twitter.com/intent/tweet?text=レベル{level}をスコア{score}でクリア！ %23" + appName + " " + appURL;
        Twitter[en] = "https://twitter.com/intent/tweet?text=Level{level} Clear! Score{score}. %23" + appName + " " + appURL;

        LINE[ja] = "http://line.me/R/msg/text/?SuperCube3のレベル{level}をスコア{score}でクリアしたよ！ " + appURLenc;
        LINE[en] = "http://line.me/R/msg/text/?SuperCube3 Level{level}Clear! Score{score}. " + appURLenc;

        Revival[ja]   = "動画を見て復活！";
        Revival[en]   = "watch the video :)";
        errRevival  = "Sorry, Not Available X(";
        failRevival = "Sorry, V4VC failed X(";

        Info[ja, 0] = "Welcome to the SuperCube World!\nスマホを左右に傾けて移動！";
        Info[ja, 1] = "画面をタップするとジャンプ！\nジャンプは2回連続できるよ。";
        Info[ja, 2] = "初めての敵だ!\nRedCubeは上から踏むと無力化できるよ。";
        Info[ja, 3] = "セーブポイントを発見!\nGreenCubeに触れるとリスポーン地点をセーブできるよ。";
        Info[ja, 4] = "Rainbow Flag に触れるとレベルクリア!\nクリア画面の「Leaderboards」でランキング、\n「Achievements」で実績が確認できるよ。";
        Info[ja, 5] = "Pink Cube!\nこいつは踏んでも無力化できない。避けて進もう。";

        Info[en, 0] = "Welcome to the SuperCube World!\nMove by tilting the device to the left and right.";
        Info[en, 1] = "Jump by tapping the screen!\nYou can jump be continuously twice.";
        Info[en, 2] = "Enemy!!\nYou can incapacitate the RedCube by touch from top.";
        Info[en, 3] = "Found the Save points!\nSpwan points will save when touch the block of Green.";
        Info[en, 4] = "Rainbow Flag!";
        Info[en, 5] = "Pink Cube!\nこいつは踏んでも無力化できない。避けて進もう。";

        Hint[ja, 0] = "HomeのSettingからジャイロ感度の変更が行えます。";
        Hint[ja, 1] = "HomeのSettingでバイブレーションやパフォーマンスの変更が行えます。";
        Hint[ja, 2] = "グリーンのブロックに触れるとセーブされます。";
        Hint[ja, 3] = "ジャンプは連続で2回できます。";
        Hint[ja, 4] = "RedCubeは上から踏むことで無力化できます。";
        Hint[ja, 5] = "PinkCubeは上から踏んでも無力化できません。";
        Hint[ja, 6] = "HomeのSettingから操作方法の変更が行えます。";

        Hint[en, 0] = "You can change the gyro sensitivity in the Setting of Home.";
        Hint[en, 1] = "You can change the vibration and performance in the Setting of Home.";
        Hint[en, 2] = "Spwan points will save when touch the block of Green.";
        Hint[en, 3] = "Jump can be twice in a row.";
        Hint[en, 4] = "You can incapacitate the RedCube by touch from top.";
        Hint[en, 5] = "You cannot incapacitate the RedCube";
        Hint[en, 6] = "You can change the control mode in the Setting of Home.";

        Setting[ja, 0] = "本体の傾きとタップで操作";
        Setting[ja, 1] = "画面上のボタンで操作";

        Setting[en, 0] = "Control by gyro and tap.";
        Setting[en, 1] = "Control by button in the display.";

        GPGSneedLogin[ja]    = "オンライン機能を利用するにはログインを行ってください。";
        GPGSsendingScore[ja] = "スコアを送信中...";
        GPGSsendScore[ja]    = "スコアの送信に成功！";
        GPGSfaildScore[ja]   = "スコアの送信に失敗。";

        GPGSneedLogin[en]    = "Online function need login.";
        GPGSsendingScore[en] = "Sending your score...";
        GPGSsendScore[en]    = "Success to send your score!";
        GPGSfaildScore[en]   = "Failed to send your score X(";

    }
}
