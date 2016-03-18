using UnityEngine;
using System.Collections;

public class WarpManager : MonoBehaviour {

    public WarpManager Target;
    public float animY;

    private Vector3 pos;
    private int mode;//0:None 1:Trans 2:Import
    private bool isEndMotion;

	// Use this for initialization
	void Start () {
        if (Target == null) UnityEngine.Debug.LogError("Warp target is NULL.");

        pos = transform.localPosition;

        mode = 0;
        animY = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

        switch (mode)
        {
            case 1:
                MotionImport();
                break;
            case 2:
                MotionTransport();
                break;
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube" && mode == 0)
        {
            CubeManager.isWarpLock = true;
            animY = 0.5f;

            mode = 1;
        }
    }

    void MotionImport()
    {
        World.Cube.transform.position = pos + new Vector3(0, animY -= 0.01f, 0);

        if (CubeManager.posY < pos.y - 1)
        {
            World.Cube.transform.position = Target.pos;
            mode = 0;

            Target.mode = 2;
            Target.animY = -1;
        }
    }

    void MotionTransport()
    {
        if (!isEndMotion) World.Cube.transform.position = pos + new Vector3(0, animY += 0.01f, 0);

        if (CubeManager.posY > pos.y + 0.4f)
        {
            isEndMotion = true;
            CubeManager.isWarpLock = false;
        }

        if (Vector3.Distance(pos, World.Cube.transform.position) > 2)
        {
            isEndMotion = false;
            mode = 0;
        }
    }
}
