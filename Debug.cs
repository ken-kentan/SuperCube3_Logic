using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Debug : MonoBehaviour {

    public GameObject parent;
    public Text StatusCube;

    // Use this for initialization
    void Start () {
        parent.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!World.isDebug) Destroy(parent);

        StatusCube.text = "Cube:\nPos(X,Y) = " + (int)CubeManager.pos.x + "," + (int)CubeManager.pos.y + "\nSpeed(X,Y) = " + (int)CubeManager.speedX + "," + (int)CubeManager.speedY +
            "\nLife = " + CubeManager.life + "\nmodeEffect = " + CubeManager.effectAqua + "," + CubeManager.effectMagnet + "," + CubeManager.effectPlusJump +
            "\n\nWorld:\nsumPoint = " + World.sumPoint + "\nsumJump = " + World.sumJump + "\nsumDead = " + World.sumDead + "\nsumKill = " + World.sumKill +
            "\nsumAqua = " + World.sumAqua + "\nsumMagnet = " + World.sumMagnet +
            "\n\nGameData:\nScore = " + GameDataManager.Get(GameDataManager.Data.Score) + "\nJump = " + GameDataManager.Get(GameDataManager.Data.Jump) +
            "\nPoint = " + GameDataManager.Get(GameDataManager.Data.Point) + "\nDead = " + GameDataManager.Get(GameDataManager.Data.Dead);
    }

    public static void Log(string str)
    {
        UnityEngine.Debug.Log(str);
    }
}
