using UnityEngine;
using System.Collections;

public class LiftManager : MonoBehaviour {

    public GameObject Lift;
    public int modeLift;//1:X 2:Y
    public bool isWakeUp;
    public float distance, speed;
    
    private Rigidbody LiftBody;
    private float speedX = 300, speedY = 180;
    private float posStart, posEnd, pos, posPrev;
    private bool mode, isOnCube, isPositive;
    private int cntStopTime;

	// Use this for initialization
	void Start () {
        World.liftManagerList.Add(this);

        LiftBody = GetComponent<Rigidbody>();

        mode = true;

        switch (modeLift)
        {
            case 1:
                pos = posStart = Lift.transform.position.x;
                if (speed != 0) speedX = speed;
                break;
            case 2:
                pos = posStart = Lift.transform.position.y;
                if (speed != 0) speedY = speed;
                break;
        }

        posEnd = posStart + distance;

        if (posStart < posEnd) isPositive = true;
        else isPositive = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause || (!isWakeUp && !isOnCube)) return;

        switch (modeLift)
        {
            case 1://X
                pos = Lift.transform.position.x;

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
                pos = Lift.transform.position.y;

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
        if (isOnCube && collision.gameObject.tag == "Cube") isOnCube = false;
    }

    void checkError()
    {
        if(Mathf.Abs(posPrev - pos) < 0.01f && cntStopTime++ > 50)
        {
            mode = !mode;
            cntStopTime = 0;
        }
        posPrev = pos;
    }

    public void Reset()
    {
        if (isWakeUp) return;

        if(modeLift == 1)
        {
            Lift.transform.position = new Vector3(posStart, Lift.transform.position.y, 0);
        }else
        {
            Lift.transform.position = new Vector3(Lift.transform.position.x, posStart, 0);
        }
    }
}
