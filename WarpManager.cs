using UnityEngine;
using System.Collections;

public class WarpManager : MonoBehaviour {

    public WarpManager Target;
    public float animY;

    private Vector3 pos;
    private enum Mode { None, Import, Spawn};
    private Mode mode;
    private bool isEndMotion;

	// Use this for initialization
	void Start () {
        if (Target == null) UnityEngine.Debug.LogError("Warp target is NULL.");

        pos = transform.localPosition;

        mode = Mode.None;
        animY = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

        switch (mode)
        {
            case Mode.Import:
                MotionImport();
                break;
            case Mode.Spawn:
                MotionSpawn();
                break;
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        if (CubeManager.isMotionDead) return;

        if (collider.gameObject.tag == "Cube" && mode == 0)
        {
            CubeManager.isWarpLock = true;
            animY = 0.5f;

            mode = Mode.Import;
        }
    }

    void MotionImport()
    {
        World.Cube.transform.position = pos + new Vector3(0, animY -= 0.01f, 0);

        if (CubeManager.posY < pos.y - 1)
        {
            World.Cube.transform.position = Target.pos;
            mode = 0;

            Target.mode = Mode.Spawn;
            Target.animY = -1.0f;
            CubeManager.UpdatePos();
        }
    }

    void MotionSpawn()
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
