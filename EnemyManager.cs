using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public enum Enemy { None, Move, StaticMove, Rotate, Shot, Drop, ShotTracking };

    public GameObject enemyCube;
    public Enemy type;
    public bool isForward, isAllowFly;
    public int cycleShot, timeStandbyDrop;
    public float distanceDrop, speedTracking;

    private Animator animator;
    private Rigidbody enemyBody;
    private SphereCollider colliderEnemyCube;
    private int cntCyclShot;
    private bool isFirst, isFly;

    private GameObject enemyParticle;
    private Vector3 posDropHome;
    private int cntStayTime;
    private bool isLockDrop;

    // Use this for initialization
    void Start () {
        World.enemyManagerList.Add(this);

        colliderEnemyCube = enemyCube.GetComponent<SphereCollider>();
        animator = enemyCube.GetComponent<Animator>();

        isFirst = true;

        switch (type)
        {
            case Enemy.Move:
                enemyBody = enemyCube.GetComponent<Rigidbody>();

                if (isForward) enemyBody.AddForce(10, 0, 0);
                else enemyBody.AddForce(-10, 0, 0);

                isFly = false;

                break;
            case Enemy.StaticMove:
                enemyBody = enemyCube.GetComponent<Rigidbody>();

                if (isForward) enemyBody.AddForce(10, 0, 0);
                else enemyBody.AddForce(-10, 0, 0);

                isFly = false;
                break;
            case Enemy.Rotate:
                if (!isForward) animator.SetFloat("Speed", -1);
                break;
            case Enemy.Shot:
            case Enemy.ShotTracking:
                cntCyclShot = timeStandbyDrop = 0;
                if (cycleShot == 0) cycleShot = 300;
                break;
            case Enemy.Drop:
                posDropHome = transform.localPosition;

                isLockDrop = true;
                cntStayTime = 0;
                if (distanceDrop == 0) distanceDrop = 6.0f;
                if (timeStandbyDrop == 0) timeStandbyDrop = 20;
                cntStayTime = timeStandbyDrop;

                enemyBody = enemyCube.GetComponent<Rigidbody>();
                enemyBody.useGravity = false;
                enemyBody.constraints = RigidbodyConstraints.FreezeAll;
                break;
        }

        if (isAllowFly)
        {
            enemyParticle = Instantiate(World.EnemyPatricle);
            enemyParticle.transform.parent = enemyCube.transform;
            enemyParticle.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (IsOverWorld()) Destroy(enemyCube);

        //return when over distance
        if (World.isPause || type == Enemy.None || (isFirst && Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance - 3)) return;

        switch (type)
        {
            case Enemy.Move:
            case Enemy.StaticMove:
                if (!IsOnFloor() && !isAllowFly)
                {
                    isFly = true;
                    break;
                }
                else
                {
                    isFly = false;
                }

                if (isAllowFly)
                {
                    enemyParticle.SetActive(true);
                    if (!isForward)
                    {
                        enemyParticle.transform.localPosition = new Vector3(0.5f, 0, 0);
                        enemyParticle.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else
                    {
                        enemyParticle.transform.localPosition = new Vector3(-0.5f, 0, 0);
                        enemyParticle.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                }

                if(Mathf.Abs(enemyBody.velocity.x) < 0.1f && cntStayTime++ > 10)
                {
                    isForward = !isForward;
                    cntStayTime = 0;
                }

                //Change move direction
                if ((IsTouchLeft() || IsTouchRight()) && !isFly)
                {
                    isForward = !isForward;
                    enemyBody.velocity = Vector3.ClampMagnitude(enemyBody.velocity, 0f);
                }

                if (isForward) enemyBody.AddForce(10, 0, 0);
                else           enemyBody.AddForce(-10, 0, 0);
                break;
            case Enemy.Rotate:
                animator.enabled = true;
                break;
            case Enemy.Shot:
                if (cntCyclShot++ % cycleShot == 0) Instantiate(World.EnemyChieldren, transform.position, transform.rotation);
                break;
            case Enemy.ShotTracking:
                if (cntCyclShot++ % cycleShot == 0)
                {
                    GameObject objectShot = Instantiate(World.EnemyTracking);
                    GameObject Sensor = objectShot.transform.FindChild("Sensor").gameObject;
                    objectShot.transform.position = transform.position;
                    Sensor.GetComponent<EnemyChildren>().speedTracking = speedTracking;
                }
                break;
            case Enemy.Drop:
                if (Mathf.Abs(posDropHome.x - World.cubeManager.pos.x) <= distanceDrop && World.cubeManager.pos.y < posDropHome.y && isLockDrop && cntStayTime++ > timeStandbyDrop) {
                    cntStayTime = 0;
                    isLockDrop = false;
                    enemyBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    enemyBody.useGravity = true;
                }

                if (!isLockDrop && ((enemyBody.velocity.y > -0.1f && cntStayTime++ > 50) || enemyBody.transform.localPosition.y < 0))
                {
                    enemyBody.useGravity = false;

                    if (transform.localPosition.y >= posDropHome.y)
                    {
                        isLockDrop = true;
                        cntStayTime = 0;
                        enemyBody.velocity = Vector3.ClampMagnitude(enemyBody.velocity, 0f);
                        enemyBody.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    else {
                        if(enemyBody.velocity.y < 0) enemyBody.velocity = Vector3.ClampMagnitude(enemyBody.velocity, 0f);
                        enemyBody.AddForce(0, 30, 0);
                    }
                }
                break;
        }
        if (isFirst) isFirst = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (type == Enemy.Drop)
        {
            World.audioSource.PlayOneShot(World.dropEnemySE);
            Vibration.Vibrate(35);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (type == Enemy.StaticMove || type == Enemy.Rotate || World.cubeManager.isMotionDead) return;

        if (collider.gameObject.tag == "Cube" && World.cubeManager.pos.y - enemyCube.transform.localPosition.y > 0.8f)
        {
            World.audioSource.PlayOneShot(World.killEnemySE);
            World.sumKill++;
            GameDataManager.AddDataValue(GameDataManager.Data.Kill);
            enemyCube.tag = "Untagged";
            World.cubeManager.ResetJump();
            World.cubeManager.cubeBody.velocity = Vector3.ClampMagnitude(World.cubeManager.cubeBody.velocity, 0f);
            World.cubeManager.cubeBody.AddForce(0, 200f, 0);
            animator.enabled = true;
            Destroy(enemyCube.GetComponent<Collider>());
            Destroy(enemyCube, 1.0f);
            Destroy(gameObject);
        }
    }

    bool IsOnFloor()
    {
        return Physics.Raycast(enemyCube.transform.position, new Vector3(0, -0.5f, 0), 1);
    }

    bool IsTouchLeft()
    {
        return Physics.Raycast(enemyCube.transform.position, new Vector3(-0.5f, 0, 0), colliderEnemyCube.radius, 9, QueryTriggerInteraction.Ignore);
    }

    bool IsTouchRight()
    {
        return Physics.Raycast(enemyCube.transform.position, new Vector3( 0.5f, 0, 0), colliderEnemyCube.radius, 9, QueryTriggerInteraction.Ignore);
    }

    bool IsOverWorld()
    {
        if (enemyCube.transform.localPosition.y < -10.0f) return true;
        return false;
    }

    public void Reset()
    {
        if(type == Enemy.Drop) enemyBody.transform.localPosition = posDropHome;
    }

    void OnDestroy()
    {
        World.enemyManagerList.Remove(this);
        Debug.Log("EnemyManager removed!");
    }
}
