using UnityEngine;
using System.Collections;

public class EnemyChildren : MonoBehaviour {

    public enum Type { Shot, Rotate , ShotTracking };

    public Type type;
    public float speedTracking;
    private GameObject parentObject;
    private Rigidbody EnemyBody;

    // Use this for initialization
    void Start() {
        World.enemyChildrenList.Add(this);

        switch (type)
        {
            case Type.Shot:
                EnemyBody = GetComponent<Rigidbody>();

                Vector3 forceDirection = World.Cube.transform.localPosition - transform.localPosition;
                float sum = (Mathf.Abs(forceDirection.x) + Mathf.Abs(forceDirection.y));

                float rate = 500.0f / sum;

                EnemyBody.AddForce(forceDirection * rate);
                break;
            case Type.Rotate:
                break;
            case Type.ShotTracking:
                parentObject = gameObject.transform.parent.gameObject;
                if (speedTracking == 0) speedTracking = 3.5f;
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if (World.isPause) return;

        switch (type)
        {
            case Type.Shot:
                if ((Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)) Destroy(gameObject);
                break;
            case Type.ShotTracking:
                if ((Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)) Destroy(parentObject);

                Vector3 parentPos = parentObject.transform.position,
                        direction = CubeManager.pos - parentPos;

                direction.Normalize();

                parentObject.transform.position = parentPos + direction * speedTracking * Time.deltaTime;
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Cube" || CubeManager.isMotionDead) return;

        switch (type)
        {
            case Type.Rotate:
                World.cubeManager.KillCube();
                break;
            case Type.ShotTracking:
                KillEnemy();
                break;
        }
    }

    void KillEnemy()
    {
        World.audioSource.PlayOneShot(World.killEnemySE);
        World.sumKill++;
        GameDataManager.AddDataValue(GameDataManager.Data.Kill);
        parentObject.tag = "Untagged";
        CubeManager.ResetJump();
        CubeManager.cubeBody.velocity = Vector3.ClampMagnitude(CubeManager.cubeBody.velocity, 0f);
        CubeManager.cubeBody.AddForce(0, 200, 0);
        parentObject.GetComponent<Animator>().enabled = true;
        Destroy(parentObject.GetComponent<Collider>());
        Destroy(parentObject, 1.0f);
        Destroy(gameObject);
    }

    public void Reset()
    {
        if(type == Type.Shot)               Destroy(gameObject);
        else if (type == Type.ShotTracking) Destroy(parentObject);
    }
}
