using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

    public enum ButtonType { OK, Other, Cancel}
    public enum Action { None, Close, OpenURL}

    public Text textTitle, textMessage;
    public Button btnOK, btnOther, btnCancel;
    public Text textOK, textOther, textCancel;

    private Action actionOK, actionOther, actionCancel;
    private string url;

    private string[] saveKey = new string[3];
    private int[] saveValue = new int[3];

    public void Init(string title, string msg, Action ok, Action other, Action cancel)
    {
        textTitle.text = title;
        textMessage.text = msg;

        actionOK = ok;
        actionOther = other;
        actionCancel = cancel;
    }

    public void SetButton(string ok, string other, string cancel, int fontSize = 70)
    {
        textOK.fontSize = textOther.fontSize = textCancel.fontSize = fontSize;
        textOK.text = ok;
        textOther.text = other;
        textCancel.text = cancel;
    }

	public void SetURL(string url)
    {
        this.url = url;
    }

    public void SetSaveData(ButtonType type, string key, int value)
    {
        saveKey[(int)type] = key;
        saveValue[(int)type] = value;
    }

    public void OnClickOK()
    {
        switch (actionOK)
        {
            case Action.OpenURL:
                Application.OpenURL(url);
                break;
            default:
                break;
        }

        if (saveKey[0] != default(string)) SaveData(ButtonType.OK);

        Destroy(gameObject);
    }

    public void OnClickOther()
    {
        if (saveKey[1] != default(string)) SaveData(ButtonType.Other);

        Destroy(gameObject);
    }

    public void OnClickCancel()
    {
        if (saveKey[2] != default(string)) SaveData(ButtonType.Cancel);

        Destroy(gameObject);
    }

    void SaveData(ButtonType type)
    {
        PlayerPrefs.SetInt(saveKey[(int)type], saveValue[(int)type]);
        PlayerPrefs.Save();

        Debug.Log("Dialog data save.(" + saveKey[(int)type] + "," + saveValue[(int)type] + ")");
    }
}
