using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CubeManager : MonoBehaviour {

    public GameObject Camera;
    public Rigidbody cubeBody;
    public Vector3 pos, speed;
    public float KaccGyro;
    public int maxJump, life;
    public bool isMotionDead, isNotStop, isWarpLock;

    private readonly float MAX_SPEED = 7.0f;
    private int cntJump, cntMotionDead;
    private bool isOnFloor, isOnBlock, isOnLift;

    private enum MOVE { NONE, LEFT, RIGHT};
    private MOVE moveMode;

    void Awake()
    {
        World.cubeManager = this;
    }

    // Use this for initialization
    void Start() {
        cubeBody = GetComponent<Rigidbody>();

        KaccGyro = PlayerPrefs.GetFloat("KaccGyro", 25.0f);

        pos = transform.position;
        speed = cubeBody.velocity;

        maxJump = 2;
        life = 3;
    }

    // Update is called once per frame
    void Update() {
        if (World.isPause || isMotionDead || isWarpLock) return;

        //jump
        if (((Input.GetMouseButtonDown(0) && !World.isController) || GameUIManager.isJump) && (isOnFloor || isOnBlock || isOnLift || cntJump < maxJump))
        {
            Jump();
        }
        GameUIManager.isJump = false;

        //move
        if (Input.GetKey("left") || GameUIManager.isLeft)        moveMode = MOVE.LEFT;
        else if (Input.GetKey("right") || GameUIManager.isRight) moveMode = MOVE.RIGHT;
        else                                                     moveMode = MOVE.NONE;
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

            if (Mathf.Abs(speed.x) < MAX_SPEED) cubeBody.AddForce(accGyro, 0f, 0f);
            else                                cubeBody.AddForce(-accGyro, 0f, 0f);

            return;
        }

        switch (moveMode)
        {
            case MOVE.LEFT:
                if (speed.x > -MAX_SPEED) cubeBody.AddForce(-9f, 0f, 0f);
                break;
            case MOVE.RIGHT:
                if (speed.x <  MAX_SPEED) cubeBody.AddForce(9f, 0f, 0f);
                break;
        }
    }

    void Jump()
    {
        World.audioSource.PlayOneShot(World.jumpSE);
        cntJump++;
        World.sumJump++;
        GameDataManager.AddDataValue(GameDataManager.Data.Jump);
        StopCube();
        cubeBody.AddForce(0f, 350.0f, 0f);
    }

    bool isOverWorld()
    {
        if (pos.y < -2.0f && !isMotionDead)
        {
            return true;
        }
        return false;
    }

    public void StopCube(bool isForce = false)
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
                if (enemyManager != null) enemyManager.Reset();
            }

            foreach(EnemyChildren enemyChildren in World.enemyChildrenList)
            {
                if (enemyChildren != null) enemyChildren.Reset();
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

    public void UpdatePos()
    {
        pos = World.Cube.transform.position;
    }

    public void ResetJump(int cnt = 0)
    {
        cntJump = cnt;
    }
}
