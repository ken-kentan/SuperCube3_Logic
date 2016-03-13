using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CubeManager : MonoBehaviour {

    public static float posX, posY, speedX, speedY, KaccGyro;
    public static int maxJump, life;
    public static int effectAqua, effectMagnet, effectPlusJump;
    public static bool isResetCube, isMotionDead;
    public static Rigidbody cubeBody;
    private static readonly float maxSpeed = 8.0f;
    private static int cntJump, cntMotionDead;
    private static bool isOnFloor, isOnBlock, isOnEnemy, isOnLift;

    // Use this for initialization
    void Start() {
        posX = transform.position.x;
        posY = transform.position.y;

        cubeBody = GetComponent<Rigidbody>();

        speedX = cubeBody.velocity.x;
        speedY = cubeBody.velocity.y;

        effectAqua = effectMagnet = effectPlusJump = 0;

        KaccGyro = PlayerPrefs.GetFloat("KaccGyro", 25.0f);

        maxJump = 2;
        cntJump = cntMotionDead = 0;

        life = 3;

        isResetCube = false;

        isOnFloor = isOnBlock = isOnEnemy = isOnLift = isMotionDead = false;
    }

    // Update is called once per frame
    void Update() {
        if (World.isPause) return;

        posX = transform.position.x;
        posY = transform.position.y;
        speedX = cubeBody.velocity.x;
        speedY = cubeBody.velocity.y;

        if (isOnEnemy || isOverWorld()) resetCube();
        else isResetCube = false;

        if (isMotionDead)
        {
            motionDead();
            return;
        }

        //jump
        if (((Input.GetMouseButtonDown(0) && !World.isController) || GameUIManager.isJump) && (isOnFloor || isOnBlock || isOnLift || cntJump < maxJump))
        {
            World.audioSource.PlayOneShot(World.jumpSE);
            cntJump++;
            World.sumJump++;
            GameDataManager.Jump++;
            GameDataManager.SaveTotal();
            stopCube();
            cubeBody.AddForce(0, 260f, 0);
        }
        GameUIManager.isJump = false;

        //Gyro ctrl
        if (!World.isController)
        {
            float accGyro = Input.acceleration.x * KaccGyro;

            if(Mathf.Abs(speedX) < maxSpeed) cubeBody.AddForce( accGyro, 0f, 0f);
            else                             cubeBody.AddForce(-accGyro, 0f, 0f);
        }

        //X move
        if (Input.GetKey("left") || GameUIManager.isLeft)
        {
            if (speedX > -maxSpeed) cubeBody.AddForce(-9f, 0, 0);
            else                    cubeBody.AddForce( 9f, 0, 0);
        }
        else if (Input.GetKey("right") || GameUIManager.isRight)
        {
            if (speedX < maxSpeed) cubeBody.AddForce( 9f, 0, 0);
            else                   cubeBody.AddForce(-9f, 0, 0);
        }
    }

    bool isOverWorld()
    {
        if (posY < -5.0f && !isMotionDead)
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
        World.audioSource.PlayOneShot(World.damageSE);
        World.sumDead++;
        Vibration.Vibrate(600);
        GameDataManager.Dead++;
        GameDataManager.SaveEnemy();
        stopCube();
        isMotionDead = true;
        isOnEnemy = false;
    }

    void motionDead()
    {
        World.Cube.GetComponent<Collider>().isTrigger = true;
        if (cntMotionDead == 0)
        {
            CubeEffects.Run.Dead();
            cubeBody.AddForce(0, 200f, 0);
        }

        if(++cntMotionDead > 120)
        {
            cntMotionDead = 0;
            isMotionDead = false;
            World.Cube.GetComponent<Collider>().isTrigger = false;
            transform.position = World.posReborn;
            isResetCube = true;

            if (life < 0) World.isGameOver = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && collision.transform.position.y < posY) isOnFloor = true;
        if (collision.gameObject.tag == "Block" && collision.transform.position.y < posY) isOnBlock = true;
        if (collision.gameObject.tag == "Lift"  && collision.transform.position.y < posY)  isOnLift = true;

        if (collision.gameObject.tag == "Enemy")
        {
            --life;
            isOnEnemy = true;
            return;
        }

        if (isOnBlock || isOnFloor || isOnLift) cntJump = 0;

        World.audioSource.PlayOneShot(World.contactSE);
        Vibration.Vibrate(35);
    }

    void OnCollisionExit(Collision collision)
    {
        if (isOnFloor) isOnFloor = false;
        if (isOnBlock) isOnBlock = false;
        if (isOnLift ) isOnLift  = false;
    }
}
