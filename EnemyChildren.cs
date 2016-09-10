using UnityEngine;
using System.Collections;

public class EnemyChildren : MonoBehaviour {

    public enum Type { Shot, Rotate , ShotTracking };

    public Type type;
    public float speedTracking;
    public int lifeTime;
    private GameObject parentObject;
    private Rigidbody EnemyBody;

    // Use this for initialization
    void Start() {
        World.enemyChildrenList.Add(this);

        switch (type)
        {
            case Type.Shot:
                lifeTime = 600;

                EnemyBody = GetComponent<Rigidbody>();

                Vector3 forceDirection = World.Cube.transform.localPosition - transform.localPosition;
                float sum = (Mathf.Abs(forceDirection.x) + Mathf.Abs(forceDirection.y));

                float rate = 500.0f / sum;

                EnemyBody.AddForce(forceDirection * rate);
                break;
            case Type.Rotate:
                break;
            case Type.ShotTracking:
                lifeTime = 300;

                parentObject = gameObject.transform.parent.gameObject;
                if (speedTracking == 0) speedTracking = 3.5f;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (World.isPause) return;

        switch (type)
        {
            case Type.Rotate:
                return;
            case Type.Shot:
                if ((Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)) Destroy(gameObject);

                if (lifeTime-- <= 100.0)
                {
                    Color color = GetComponent<Renderer>().material.color;
                    color.a = lifeTime / 100.0f;
                    GetComponent<Renderer>().material.color = color;

                    if (lifeTime <= 0) Destroy(gameObject);
                }
                break;
            case Type.ShotTracking:
                if ((Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)) Destroy(parentObject);

                Vector3 parentPos = parentObject.transform.position,
                        direction = World.cubeManager.pos - parentPos;

                direction.Normalize();

                parentObject.transform.position = parentPos + direction * speedTracking * Time.deltaTime;

                if (lifeTime-- <= 100.0)
                {
                    Color color = parentObject.GetComponent<Renderer>().material.color;
                    color.a = lifeTime / 100.0f;
                    parentObject.GetComponent<Renderer>().material.color = color;

                    if (lifeTime <= 0) Destroy(parentObject);
                }
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Cube" || World.cubeManager.isMotionDead) return;

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
        World.cubeManager.ResetJump();
        World.cubeManager.cubeBody.velocity = Vector3.ClampMagnitude(World.cubeManager.cubeBody.velocity, 0f);
        World.cubeManager.cubeBody.AddForce(0, 200, 0);
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

    void OnDestroy()
    {
        World.enemyChildrenList.Remove(this);
        Debug.Log("EnemyChildren removed!");
    }
}
