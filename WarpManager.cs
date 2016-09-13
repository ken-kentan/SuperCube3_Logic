using UnityEngine;
using System.Collections;

public class WarpManager : MonoBehaviour {

    public enum SpawnObject { None, EnemyMove, EnemyStaticMove, SpringBlock}

    public WarpManager Target;
    public SpawnObject type;
    public float distanceSpawn, distanceResetSpawn, dragEnemy;
    public bool isEnemyForward;
    public float SpringForce;

    private GameObject objectSpawned;

    private Vector3 pos, posSpawn, posCube, motion;
    private enum Mode { None, Import, Spawn};
    private Mode mode;
    private bool isStayCube;
    private bool isEndSpawnMotion, isSpawned;

	// Use this for initialization
	void Start () {
        if (Target == null && type == SpawnObject.None) Debug.LogError("Warp target is null, SpawnObject is None.");

        pos = transform.position;

        if (dragEnemy == 0) dragEnemy = 1;
        if (SpringForce == 0) SpringForce = 300;
        mode = Mode.None;

        switch ((int)transform.eulerAngles.z)
        {
            case 0:
                posSpawn = pos + new Vector3(0, 0.5f, 0);
                pos -= new Vector3(0, 1, 0);
                motion = new Vector3(0, 0.015f, 0);
                break;
            case 270:
                posSpawn = pos + new Vector3(-0.5f, 0, 0);
                pos -= new Vector3(-1, 0, 0);
                motion = new Vector3(-0.015f, 0, 0);
                break;
            case 180:
                posSpawn = pos + new Vector3(0, -0.5f, 0);
                pos -= new Vector3(0, -1, 0);
                motion = new Vector3(0, -0.015f, 0);
                break;
            case 90:
                posSpawn = pos + new Vector3(0.5f, 0, 0);
                pos -= new Vector3(1, 0, 0);
                motion = new Vector3(0.015f, 0, 0);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

        if (type != SpawnObject.None && mode == Mode.None && Vector3.Distance(World.cubeManager.pos, transform.position) < distanceSpawn)
        {
            if (!isSpawned && !isStayCube)
            {
                isSpawned = true;
                switch (type)
                {
                    case SpawnObject.EnemyMove:
                        objectSpawned = Instantiate(World.EnemyMove);
                        GameObject Sensor = objectSpawned.transform.FindChild("Sensor").gameObject;
                        Sensor.GetComponent<EnemyManager>().isForward = isEnemyForward;
                        break;
                    case SpawnObject.EnemyStaticMove:
                        objectSpawned = Instantiate(World.EnemyStaticMove);
                        objectSpawned.GetComponent<EnemyManager>().isForward = isEnemyForward;
                        break;
                    case SpawnObject.SpringBlock:
                        objectSpawned = Instantiate(World.SpringBlock);
                        objectSpawned.GetComponent<SpringManager>().force = SpringForce;
                        break;
                }

                objectSpawned.GetComponent<Rigidbody>().drag = dragEnemy;
                objectSpawned.transform.position = posSpawn;
            }
            else
            {
                if (objectSpawned == null || (objectSpawned != null && distanceResetSpawn != 0 && Vector3.Distance(pos, objectSpawned.transform.position) > distanceResetSpawn)) isSpawned = false;
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

    void OnCollisionEnter(Collision collision)
    {
        if (World.cubeManager.isMotionDead || Target == null) return;

        if (collision.gameObject.tag == "Cube" && mode == 0)
        {
            World.cubeManager.isWarpLock = true;
            isStayCube = true;

            mode = Mode.Import;
            World.Cube.transform.position = posCube = posSpawn;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isStayCube = false;
    }

    void MotionImport()
    {
        posCube -= motion;

        World.Cube.transform.position = posCube;

        if (Vector3.Distance(posCube, pos) < 0.002f)
        {
            World.Cube.transform.position = Target.posCube = Target.pos;

            mode = Mode.None;
            Target.mode = Mode.Spawn;
        }
    }

    void MotionSpawn()
    {
        if (!isEndSpawnMotion)
        {
            posCube += motion;
            World.Cube.transform.position = posCube;
        }

        if (Vector3.Distance(posCube, posSpawn) < 0.002f)
        {
            isEndSpawnMotion = true;
            World.cubeManager.isWarpLock = false;
        }

        World.cubeManager.UpdatePos();

        if (Vector3.Distance(posSpawn, World.cubeManager.pos) > 2.0f)
        {
            isEndSpawnMotion = false;
            mode = Mode.None;
        }
    }
}
