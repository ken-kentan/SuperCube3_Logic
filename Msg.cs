using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Msg : MonoBehaviour {

    public static readonly int JA = 0, EN = 1;
    
    public static int typeLang;
    
    public const string appName   = "SuperCube_World",
                        appVer    = "1.3.20",
                        appURL    = "http://ken.kentan.jp/SuperCube_World",
                        appURLenc = "http%3a%2f%2fken%2ekentan%2ejp%2fSuperCube_World";
    public static string[] Twitter = new string[2],
                           LINE    = new string[2],
                           Revival = new string[2];
    public static string errRevival, failRevival;
    public static string[,] Info = new string[2, 8];
    public static string[,] Hint = new string[2, 7];
    public static string[,] Setting = new string[2, 2];
    public static string[] GPGSneedLogin    = new string[2],
                           GPGSsendingScore = new string[2],
                           GPGSsendScore    = new string[2],
                           GPGSfaildScore   = new string[2];
    public static string[,] Review = new string[2, 4];
    public static string[,] Store = new string[2, 2];

    // Use this for initialization
    void Start () {
        if (Application.systemLanguage == SystemLanguage.Japanese) typeLang = JA;
        else                                                       typeLang = EN;

        Twitter[JA] = "https://twitter.com/intent/tweet?text=レベル{level}をスコア{score}でクリア！ %23" + appName + " " + appURL;
        Twitter[EN] = "https://twitter.com/intent/tweet?text=Level{level} Clear! Score{score}. %23" + appName + " " + appURL;

        LINE[JA] = "http://line.me/R/msg/text/?SuperCube3のレベル{level}をスコア{score}でクリアしたよ！ " + appURLenc;
        LINE[EN] = "http://line.me/R/msg/text/?SuperCube3 Level{level}Clear! Score{score}. " + appURLenc;

        Revival[JA]   = "動画を見て復活！";
        Revival[EN]   = "watch the video :)";
        errRevival  = "Sorry, Not Available X(";
        failRevival = "Sorry, V4VC failed X(";

        Info[JA, 0] = "Welcome to the SuperCube World!\nボタンで左右に移動！\nまた、操作方法はSettingから変更できるよ。";
        Info[JA, 1] = "▲をタップするとジャンプ！\nジャンプは2回連続できるよ。";
        Info[JA, 2] = "初めての敵だ!\nRedCubeは上から踏むと無力化できるよ。";
        Info[JA, 3] = "セーブポイントを発見!\nGreenCubeに触れるとリスポーン地点をセーブできるよ。";
        Info[JA, 4] = "Goal Flag に触れるとレベルクリア!\nクリア画面の「Leaderboards」でランキング、\n「Achievements」で実績が確認できるよ。";
        Info[JA, 5] = "Pink Cube!\nこいつは踏んでも無力化できない。避けて進もう。";
        Info[JA, 6] = "Item Blockを見つけた!\nアイテムブロックを下から押すとアイテムが出現するよ！";
        Info[JA, 7] = "Spring Blockを見つけた!\n上から踏むと跳ねることができる。\n横から押せば動かせるよ。";

        Info[EN, 0] = "Welcome to the SuperCube World!\nMove by push the button to the left and right.";
        Info[EN, 1] = "Jump by tapping the ▲!\nYou can press twice to double jump.";
        Info[EN, 2] = "Enemy!!\nYou can incapacitate the Red Cube by jumping on top of it.";
        Info[EN, 3] = "Found a Spawn point!\n The Green Blocks will save your progress when you touch them.";
        Info[EN, 4] = "Goal Flag!";
        Info[EN, 5] = "Pink Cube!\nIt's invincible... So, avoid the Pink Cube.";
        Info[EN, 6] = "Found the Item Block!\nItems appear when you push under this block.";
        Info[EN, 7] = "Found the Spring Block!\nJump on this block to bounce.\nYou can also move it by pushing from the side.";

        Hint[JA, 0] = "HomeのSettingからジャイロ感度の変更が行えます。";
        Hint[JA, 1] = "HomeのSettingでバイブレーションやパフォーマンスの変更が行えます。";
        Hint[JA, 2] = "グリーンのブロックに触れるとセーブされます。";
        Hint[JA, 3] = "ジャンプは連続で2回できます。";
        Hint[JA, 4] = "RedCubeは上から踏むことで無力化できます。";
        Hint[JA, 5] = "PinkCubeは上から踏んでも無力化できません。";
        Hint[JA, 6] = "HomeのSettingから操作方法(ジャイロ、コントローラー)の変更が行えます。";

        Hint[EN, 0] = "You can change the gyro sensitivity in the Home Setting.";
        Hint[EN, 1] = "You can change the vibration and performance in the Home Setting.";
        Hint[EN, 2] = "Spawn points will be saved when you touch the Green Blocks.";
        Hint[EN, 3] = "You can double jump by tapping twice.";
        Hint[EN, 4] = "You can incapacitate the Red Cube by jumping on top of it.";
        Hint[EN, 5] = "You cannot incapacitate the RedCube";
        Hint[EN, 6] = "You can change the control mode in the Home Setting.";

        Setting[JA, 0] = "本体の傾きとタップで操作";
        Setting[JA, 1] = "画面のボタンで操作";

        Setting[EN, 0] = "Control by gyro and tap.";
        Setting[EN, 1] = "Control by button in the display.";

        GPGSneedLogin[JA]    = "オンライン機能を利用するにはログインしてください。";
        GPGSsendingScore[JA] = "スコアを送信中...";
        GPGSsendScore[JA]    = "スコアの送信に成功！";
        GPGSfaildScore[JA]   = "スコアの送信に失敗";

        GPGSneedLogin[EN]    = "Online function need login.";
        GPGSsendingScore[EN] = "Sending your score...";
        GPGSsendScore[EN]    = "Successfully sent your score!";
        GPGSfaildScore[EN]   = "Failed to send your score X(";

        Review[JA, 0] = "ゲームをプレイしていただきありがとうございます！\r\n機能追加や改善につなげていくために、レビューを書いて頂ければ幸いです。\r\nご協力をお願い致します。";
        Review[JA, 1] = "評価する";
        Review[JA, 2] = "後で";
        Review[JA, 3] = "キャンセル";

        Review[EN, 0] = "If you enjoy playing Super Cube World, would you mind taking a moment to rate it?\r\nIt won't take more than a minute.\r\nThanks for your support!";
        Review[EN, 1] = "Rate It Now";
        Review[EN, 2] = "Remind Me Later";
        Review[EN, 3] = "No, Thanks";

        Store[JA, 0] = "Cubeのライフを追加";
        Store[JA, 1] = "Cubeのジャンプ力を強化";

        Store[EN, 0] = "Upgrade the default life of Cube.";
        Store[EN, 1] = "Upgrade the Jump power of Cube.";
    }
}
