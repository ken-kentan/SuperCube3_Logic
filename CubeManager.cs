using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CubeManager : MonoBehaviour {

    public GameObject Camera;
    public static Rigidbody cubeBody;
    public static Vector3 pos;
    public static float speedX, speedY, KaccGyro;
    public static int maxJump, life;
    public static bool isResetCube, isMotionDead, isNotStop, isWarpLock;

    private static readonly float maxSpeed = 8.0f;
    private static int cntJump, cntMotionDead;
    private static bool isOnFloor, isOnBlock, isOnEnemy, isOnLift;

    // Use this for initialization
    void Start() {
        cubeBody = GetComponent<Rigidbody>();

        pos = transform.position;

        speedX = cubeBody.velocity.x;
        speedY = cubeBody.velocity.y;

        KaccGyro = PlayerPrefs.GetFloat("KaccGyro", 25.0f);

        maxJump = 2;
        cntJump = cntMotionDead = 0;

        life = 3;

        isResetCube = isWarpLock = false;

        isOnFloor = isOnBlock = isOnEnemy = isOnLift = isMotionDead = false;
    }

    // Update is called once per frame
    void Update() {
        if (World.isPause) return;

        pos = transform.position;
        speedX = cubeBody.velocity.x;
        speedY = cubeBody.velocity.y;

        if (isOnEnemy || isOverWorld()) resetCube();
        else isResetCube = false;

        if (isMotionDead)
        {
            motionDead();
            return;
        }

        if (isWarpLock) return;

        //jump
        if (((Input.GetMouseButtonDown(0) && !World.isController) || GameUIManager.isJump) && (isOnFloor || isOnBlock || isOnLift || cntJump < maxJump))
        {
            World.audioSource.PlayOneShot(World.jumpSE);
            cntJump++;
            World.sumJump++;
            GameDataManager.AddDataValue(GameDataManager.Data.Jump);
            StopCube();
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
        if (pos.y < -2.0f && !isMotionDead)
        {
            --life;
            return true;
        }
        return false;
    }

    public static void StopCube(bool isForce = false)
    {
        if (!isNotStop || isForce) cubeBody.velocity = Vector3.ClampMagnitude(cubeBody.velocity, 0f);
        isNotStop = false;
    }

    void resetCube()
    {
        World.audioSource.PlayOneShot(World.damageSE);
        World.sumDead++;
        Vibration.Vibrate(600);
        GameDataManager.AddDataValue(GameDataManager.Data.Dead);
        StopCube(true);
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

            Camera.transform.parent = null;
            
            ServerBridge.ken_kentan_jp.GameEvent("Dead");
        }

        if(++cntMotionDead > 130)
        {
            cntMotionDead = 0;
            isMotionDead = false;
            GameUIManager.isJump = false;
            World.Cube.GetComponent<Collider>().isTrigger = false;
            transform.position = World.posReborn;
            isResetCube = true;

            CubeEffects.Run.ResetEffect();

            Camera.transform.parent = World.Cube.transform;
            Camera.transform.localPosition = new Vector3(0, 6, -15);

            if (life < 0)
            {
                World.audioVolume(0.0f);
                World.isGameOver = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && collision.transform.position.y < pos.y + 0.7f) isOnFloor = true;
        if (collision.gameObject.tag == "Block" && collision.transform.position.y < pos.y + 0.7f) isOnBlock = true;
        if (collision.gameObject.tag == "Lift"  && collision.transform.position.y < pos.y + 0.7f)  isOnLift = true;
        
        if (collision.gameObject.tag == "Enemy" && !isWarpLock)
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

    public static void Kill()
    {
        if (!isMotionDead && !isOnEnemy)
        {
            --life;
            isOnEnemy = true;
        }
    }

    public static void UpdatePos()
    {
        pos = World.Cube.transform.position;
    }

    public static void ResetJump(int cnt = 0)
    {
        cntJump = cnt;
    }
}
