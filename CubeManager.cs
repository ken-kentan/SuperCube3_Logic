using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CubeManager : MonoBehaviour {

    public static float posX, posY, speedX, speedY;
    public static int maxJump, life;
    public static int effectAqua, effectMagnet;
    public Rigidbody cubeBody;
    public AudioClip jumpSE, contactSE, damageSE;
    private float maxSpeed;
    private static int cntJump;
    private bool isOnFloor, isOnBlock, isOnEnemy;

    // Use this for initialization
    void Start() {
        posX = transform.position.x;
        posY = transform.position.y;

        speedX = cubeBody.velocity.x;
        speedY = cubeBody.velocity.y;

        effectAqua = effectMagnet = 0;

        maxSpeed = 8.0f;
        
        maxJump = 2;
        cntJump = 0;

        life = 3;

        isOnFloor = false;
    }

    // Update is called once per frame
    void Update() {
        posX = transform.position.x;
        posY = transform.position.y;
        speedX = cubeBody.velocity.x;
        speedY = cubeBody.velocity.y;

        if (isOnEnemy || isOverWorld()) resetCube();

        //jump
        if (Input.GetMouseButtonDown(0) && (isOnFloor || isOnBlock || cntJump < maxJump))
        {
            World.audioSource.PlayOneShot(jumpSE);
            cntJump++;
            stopCube();
            transform.GetComponent<Rigidbody>().AddForce(0, 260f, 0);
        }

        //Gyro ctrl
        cubeBody.AddForce(Input.acceleration.x * 25.0f, 0f, 0f);

        //X move
        if (Input.GetKey("left"))
        {
            if (speedX > -maxSpeed) cubeBody.AddForce(-9f, 0, 0);
            else                    cubeBody.AddForce(5f, 0, 0);
        }
        else if (Input.GetKey("right"))
        {
            if (speedX < maxSpeed) cubeBody.AddForce(9f, 0, 0);
            else                   cubeBody.AddForce(-5f, 0, 0);
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(cubeBody.velocity.x) > maxSpeed)
        {
            cubeBody.velocity = Vector3.ClampMagnitude(cubeBody.velocity, maxSpeed);
        }
    }

    bool isOverWorld()
    {
        if (posY < -10.0f)
        {
            --life;
            return true;
        }
        return false;
    }

    void stopCube()
    {
        cubeBody.velocity = Vector3.ClampMagnitude(cubeBody.velocity, 0f);
    }

    void resetCube()
    {
        if (life < 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        World.audioSource.PlayOneShot(damageSE);
        transform.position = new Vector3(0f, 2.0f, 0f);
        isOnEnemy = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") isOnFloor = true;
        if (collision.gameObject.tag == "Block") isOnBlock = true;

        if(collision.gameObject.tag == "Enemy")
        {
            --life;
            isOnEnemy = true;
        }

        if (isOnBlock || isOnFloor)
        {
            World.audioSource.PlayOneShot(contactSE);
            cntJump = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (isOnFloor) isOnFloor = false;
        if (isOnBlock) isOnBlock = false;
    }
}
