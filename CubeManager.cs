using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CubeManager : MonoBehaviour {

    public GameObject Camera;
    public static Rigidbody cubeBody;
    public static Vector3 pos, speed;
    public static float KaccGyro;
    public static int maxJump, life;
    public static bool isMotionDead, isNotStop, isWarpLock;

    private static readonly float maxSpeed = 8.0f;
    private static int cntJump, cntMotionDead;
    private static bool isOnFloor, isOnBlock, isOnLift;
    private bool isMoveLeft, isMoveRight;

    private float x, y, z;

    // Use this for initialization
    void Start() {
        World.cubeManager = this;

        cubeBody = GetComponent<Rigidbody>();

        pos = transform.position;

        speed = cubeBody.velocity;

        KaccGyro = PlayerPrefs.GetFloat("KaccGyro", 25.0f);

        maxJump = 2;
        cntJump = cntMotionDead = 0;

        life = 3;

        isWarpLock = false;

        isOnFloor = isOnBlock = isOnLift = isMotionDead = false;
    }

    // Update is called once per frame
    void Update() {
        if (World.isPause || isMotionDead || isWarpLock) return;

        //jump
        if (((Input.GetMouseButtonDown(0) && !World.isController) || GameUIManager.isJump) && (isOnFloor || isOnBlock || isOnLift || cntJump < maxJump))
        {
            World.audioSource.PlayOneShot(World.jumpSE);
            cntJump++;
            World.sumJump++;
            GameDataManager.AddDataValue(GameDataManager.Data.Jump);
            StopCube();
            y = 280f;
        }
        GameUIManager.isJump = false;

        //move
        if (Input.GetKey("left") || GameUIManager.isLeft) isMoveLeft = true;
        else                                              isMoveLeft = false;

        if (Input.GetKey("right") || GameUIManager.isRight) isMoveRight = true;
        else                                                isMoveRight = false;
    }

    void FixedUpdate()
    {
        pos = transform.position;
        speed = cubeBody.velocity;

        if (World.isPause || isWarpLock) return;

        if (isOverWorld()) KillCube();

        if (isMotionDead)
        {
            MotionDead();
            return;
        }

        //Gyro ctrl
        if (!World.isController)
        {
            float accGyro = Input.acceleration.x * KaccGyro;

            if (Mathf.Abs(speed.x) < maxSpeed) cubeBody.AddForce( accGyro, 0f, 0f);
            else cubeBody.AddForce( -accGyro, 0f, 0f);
        }

        if (isMoveLeft)
        {
            if (speed.x > -maxSpeed)
            {
                cubeBody.AddForce(-9f, 0f, 0f);
            }
            else
            {
                cubeBody.AddForce(9f, 0f, 0f);
            }
        }else if (isMoveRight)
        {
            if (speed.x < maxSpeed)
            {
                cubeBody.AddForce(9f, 0f, 0f);
            }
            else
            {
                cubeBody.AddForce(-9f, 0f, 0f);
            }
        }

        cubeBody.AddForce(0f, y, 0f);

        y = 0f;
    }

    bool isOverWorld()
    {
        if (pos.y < -2.0f && !isMotionDead)
        {
            return true;
        }
        return false;
    }

    public static void StopCube(bool isForce = false)
    {
        if (!isNotStop || isForce) cubeBody.velocity = Vector3.ClampMagnitude(cubeBody.velocity, 0f);
        isNotStop = false;
    }

    public void KillCube()
    {
        World.audioSource.PlayOneShot(World.damageSE);
        World.sumDead++;
        Vibration.Vibrate(600);
        GameDataManager.AddDataValue(GameDataManager.Data.Dead);
        StopCube(true);
        isMotionDead = true;
    }

    void MotionDead()
    {
        World.Cube.GetComponent<Collider>().isTrigger = true;

        if (cntMotionDead == 0)
        {
            World.effect.Dead();
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

            foreach(LiftManager liftManager in World.liftManagerList)
            {
                liftManager.Reset();
            }

            foreach(FloorVanish floorVanish in World.floorVanishList)
            {
                floorVanish.Reset();
            }

            foreach(EnemyManager enemyManager in World.enemyManagerList)
            {
                enemyManager.Reset();
            }

            foreach(EnemyChildren enemyChildren in World.enemyChildrenList)
            {
                enemyChildren.Reset();
            }

            --life;

            World.effect.Reset();

            transform.position = World.posReborn;

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
            KillCube();
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

    public static void UpdatePos()
    {
        pos = World.Cube.transform.position;
    }

    public static void ResetJump(int cnt = 0)
    {
        cntJump = cnt;
    }
}
