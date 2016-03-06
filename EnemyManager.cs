using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public GameObject enemyCube;
    public bool isOpposite;
    public int cycleShot;
    private Animator animator;
    private Rigidbody enemyBody;
    private SphereCollider colliderEnemyCube;
    private int modeEnemy, cntCyclShot;
    private bool isFirst, isFly;

    // Use this for initialization
    void Start () {
        colliderEnemyCube = enemyCube.GetComponent<SphereCollider>();
        animator = enemyCube.GetComponent<Animator>();

        isFirst = true;

        switch (enemyCube.ToString())
        {
            case "EnemyMove (UnityEngine.GameObject)":
                modeEnemy = 1;
                enemyBody = enemyCube.GetComponent<Rigidbody>();

                if (isOpposite) enemyBody.AddForce(10, 0, 0);
                else enemyBody.AddForce(-10, 0, 0);

                isFly = false;

                break;
            case "EnemyStaticMove (UnityEngine.GameObject)":
                modeEnemy = 2;
                enemyBody = enemyCube.GetComponent<Rigidbody>();

                if (isOpposite) enemyBody.AddForce(10, 0, 0);
                else enemyBody.AddForce(-10, 0, 0);
                break;
            case "EnemyRotate (UnityEngine.GameObject)":
                modeEnemy = 3;
                break;
            case "EnemyShot (UnityEngine.GameObject)":
                modeEnemy = 4;
                cntCyclShot = 0;
                if (cycleShot == 0) cycleShot = 300;
                break;
            default:
                modeEnemy = 0;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //return when over distance
        if (World.isPause || modeEnemy == 0 || ((isFirst || modeEnemy == 2) && Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance - 3)) return;

        switch (modeEnemy)
        {
            case 1:
            case 2:
                if (!isOnFloor() && modeEnemy == 1)
                {
                    isFly = true;
                    break;
                }

                //Change move direction
                if (Mathf.Abs(enemyBody.velocity.x) < 0.003f && !isFly)
                {
                    isOpposite = !isOpposite;
                    enemyBody.velocity = Vector3.ClampMagnitude(enemyBody.velocity, 0f);
                }
                else if(Mathf.Abs(enemyBody.velocity.x) > 2)
                {
                    isFly = false;
                }

                if (isOpposite) enemyBody.AddForce(10, 0, 0);
                else            enemyBody.AddForce(-10, 0, 0);
                break;
            case 3:
                animator.enabled = true;
                break;
            case 4:
                if (cntCyclShot++ % cycleShot == 0) Instantiate(World.EnemyChieldren, transform.position, transform.rotation);
                break;
        }
        

        if (isOverWorld()) Destroy(enemyCube);
        if (isFirst) isFirst = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (modeEnemy == 2 || modeEnemy == 3) return;

        if (collider.gameObject.tag == "Cube" && CubeManager.posY - enemyCube.transform.localPosition.y > 0.8f)
        {
            World.audioSource.PlayOneShot(World.killEnemySE);
            World.sumKill++;
            GameDataManager.Kill++;
            GameDataManager.SaveEnemy();
            enemyCube.tag = "Untagged";
            CubeManager.cubeBody.velocity = Vector3.ClampMagnitude(CubeManager.cubeBody.velocity, 0f);
            CubeManager.cubeBody.AddForce(0, 200, 0);
            animator.enabled = true;
            Destroy(enemyCube.GetComponent<Collider>());
            Destroy(enemyCube, 1.0f);
            Destroy(gameObject);
        }
    }

    bool isOnFloor()
    {
        return Physics.Raycast(enemyCube.transform.position, new Vector3(0, -0.5f, 0), colliderEnemyCube.radius);
    }

    bool isOverWorld()
    {
        if (enemyCube.transform.localPosition.y < -10.0f) return true;
        return false;
    }
}
