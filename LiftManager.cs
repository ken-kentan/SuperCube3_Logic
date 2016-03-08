using UnityEngine;
using System.Collections;

public class LiftManager : MonoBehaviour {

    public GameObject Lift;
    public int modeLift;//1:X 2:Y
    public bool isWakeUp;
    public float distance;
    
    private Rigidbody LiftBody;
    private const float speedX = 300, speedY = 180;
    private float posStart, posEnd, pos, posPrev;
    private bool mode, isOnCube, isPositive;
    private int cnt;

	// Use this for initialization
	void Start () {
        LiftBody = GetComponent<Rigidbody>();

        mode = true;

        switch (modeLift)
        {
            case 1:
                pos = posStart = Lift.transform.localPosition.x;
                break;
            case 2:
                pos = posStart = Lift.transform.localPosition.y;
                break;
        }

        posEnd = posStart + distance;

        if (posStart < posEnd) isPositive = true;
        else isPositive = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause || (!isWakeUp && !isOnCube && !CubeManager.isResetCube)) return;

        switch (modeLift)
        {
            case 1://X
                if (!isWakeUp && CubeManager.isResetCube) Lift.transform.localPosition = new Vector3(posStart, Lift.transform.localPosition.y, 0);

                pos = Lift.transform.localPosition.x;

                checkError();

                if (isPositive)
                {
                    if(pos <= posStart)    mode = true;
                    else if(posEnd <= pos) mode = false;
                }
                else
                {
                    if(posStart <= pos)    mode = false;
                    else if(pos <= posEnd) mode = true;
                }

                if (mode) LiftBody.AddForce( speedX, 0, 0);
                else      LiftBody.AddForce(-speedX, 0, 0);
                break;
            case 2://Y
                if (!isWakeUp && CubeManager.isResetCube) Lift.transform.localPosition = new Vector3(Lift.transform.localPosition.x, posStart, 0) ;

                pos = Lift.transform.localPosition.y;

                checkError();

                if (isPositive)
                {
                    if (pos <= posStart)    mode = true;
                    else if (posEnd <= pos) mode = false;
                }
                else
                {
                    if (posStart <= pos)    mode = false;
                    else if (pos <= posEnd) mode = true;
                }

                if (mode) LiftBody.AddForce(0,  speedY, 0);
                else      LiftBody.AddForce(0, -speedY, 0);
                break;

        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube") isOnCube = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (isOnCube) isOnCube = false;
    }

    void checkError()
    {
        if(Mathf.Abs(posPrev - pos) < 0.015f && cnt++ > 50)
        {
            mode = !mode;
            cnt = 0;
        }
        posPrev = pos;
    }
}
