﻿using UnityEngine;
using System.Collections;

public class WarpManager : MonoBehaviour {

    public enum SpawnObject { None, EnemyMove, EnemyStaticMove, SpringBlock}

    public WarpManager Target;
    public SpawnObject type;
    public float distanceSpawn, distanceResetSpawn, dragEnemy;
    public bool isEnemyForward;
    public float SpringForce;

    private GameObject objectSpawned;

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
        if (SpringForce == 0) SpringForce = 300;
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
                motion = new Vector3(0.1f, 0, 0);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

        if (type != SpawnObject.None && Vector3.Distance(CubeManager.pos, transform.position) < distanceSpawn)
        {
            if (!isSpawned)
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
