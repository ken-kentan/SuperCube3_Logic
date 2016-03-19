using UnityEngine;
using System.Collections;

public class WarpManager : MonoBehaviour {

    public WarpManager Target;
    public EnemyManager.Enemy type;
    public float distanceSpawn, distanceResetSpawn, dragEnemy;
    public bool isEnemyForward;

    private GameObject Enemy;

    private Vector3 pos, posSpawn, motion;
    private enum Mode { None, Import, Spawn};
    private enum Angle { Top, Bottom, Left, Right};
    private Mode mode;
    private Angle angle;
    private bool isEndSpawnMotion, isSpawned;
    private bool isTop, isBottom, isLeft, isRight;

	// Use this for initialization
	void Start () {
        if (Target == null) UnityEngine.Debug.LogWarning("Warp target is NULL.");

        pos = transform.position;

        if (dragEnemy == 0) dragEnemy = 1;
        mode = Mode.None;

        switch ((int)transform.eulerAngles.z)
        {
            case 0:
                angle = Angle.Top;
                posSpawn = pos + new Vector3(0, 0.5f, 0);
                pos -= new Vector3(0, 1, 0);
                motion = new Vector3(0, 0.01f, 0);
                break;
            case 90:
                angle = Angle.Right;
                posSpawn = pos + new Vector3(-0.5f, 0, 0);
                pos -= new Vector3(-1, 0, 0);
                motion = new Vector3(-0.01f, 0, 0);
                break;
            case 180:
                angle = Angle.Bottom;
                posSpawn = pos + new Vector3(0, -0.5f, 0);
                pos -= new Vector3(0, -1, 0);
                motion = new Vector3(0, -0.01f, 0);
                break;
            case 270:
                angle = Angle.Left;
                posSpawn = pos + new Vector3(0.5f, 0, 0);
                pos -= new Vector3(1, 0, 0);
                motion = new Vector3(0, 0, 0.01f);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

        if (type != EnemyManager.Enemy.None && Vector3.Distance(CubeManager.pos, transform.position) < distanceSpawn)
        {
            if (!isSpawned)
            {
                isSpawned = true;
                switch (type)
                {
                    case EnemyManager.Enemy.Move:
                        Enemy = Instantiate(World.EnemyMove);
                        GameObject Sensor = Enemy.transform.FindChild("Sensor").gameObject;
                        Sensor.GetComponent<EnemyManager>().isForward = isEnemyForward;
                        break;
                    case EnemyManager.Enemy.StaticMove:
                        Enemy = Instantiate(World.EnemyStaticMove);
                        Enemy.GetComponent<EnemyManager>().isForward = isEnemyForward;
                        break;
                }

                Enemy.GetComponent<Rigidbody>().drag = dragEnemy;
                Enemy.transform.position = posSpawn;
            }
            else
            {
                if (Enemy == null || (Enemy != null && distanceResetSpawn != 0 && Vector3.Distance(pos, Enemy.transform.position) > distanceResetSpawn)) isSpawned = false;
            }
        }

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
        if (CubeManager.isMotionDead || Target == null) return;

        if (collider.gameObject.tag == "Cube" && mode == 0)
        {
            CubeManager.isWarpLock = true;

            mode = Mode.Import;
            World.Cube.transform.position = posSpawn;
            CubeManager.UpdatePos();
        }
    }

    void MotionImport()
    {
        World.Cube.transform.position = CubeManager.pos - motion;

        if (Vector3.Distance(CubeManager.pos, pos) < 0.001f)
        {
            World.Cube.transform.position = Target.pos;
            mode = 0;

            Target.mode = Mode.Spawn;
            CubeManager.UpdatePos();
        }
    }

    void MotionSpawn()
    {
        if (!isEndSpawnMotion) World.Cube.transform.position = CubeManager.pos + motion;

        if (Vector3.Distance(CubeManager.pos, posSpawn) < 0.001f)
        {
            isEndSpawnMotion = true;
            CubeManager.isWarpLock = false;
        }

        if (Vector3.Distance(posSpawn, CubeManager.pos) > 2)
        {
            isEndSpawnMotion = false;
            mode = 0;
        }
    }
}
