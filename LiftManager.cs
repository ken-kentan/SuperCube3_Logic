using UnityEngine;
using System.Collections;

public class LiftManager : MonoBehaviour {

    public GameObject Lift;
    public int modeLift;//1:X 2:Y
    public bool isWakeUp;
    public float distance;
    
    private Rigidbody LiftBody;
    private float posStart, posEnd, pos, force;
    private bool mode, isOnCube;

	// Use this for initialization
	void Start () {
        LiftBody = GetComponent<Rigidbody>();

        mode = true;

        switch (modeLift)
        {
            case 1:
                pos = posStart = Lift.transform.localPosition.x;
                posEnd = posStart + distance;

                if (posStart < posEnd) force = 200;
                else                   force = -200;
                break;
            case 2:
                pos = posStart = Lift.transform.localPosition.y;
                posEnd = posStart + distance;

                if (posStart < posEnd) force =  200;
                else                   force = -200;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause || (!isWakeUp && !isOnCube)) return;

        switch (modeLift)
        {
            case 1:
                pos = Lift.transform.localPosition.x;

                if (Mathf.Abs(pos - posStart) < 0.02f || Mathf.Abs(pos - posEnd) < 0.02f)
                {
                    LiftBody.velocity = Vector3.ClampMagnitude(LiftBody.velocity, 0f);
                    if (Mathf.Abs(pos - posStart) < 0.02f) mode = true;
                    else                                   mode = false;
                }

                if (mode) LiftBody.AddForce( force, 0, 0);
                else      LiftBody.AddForce(-force, 0, 0);
                break;
            case 2:
                pos = Lift.transform.localPosition.y;

                if (Mathf.Abs(pos - posStart) < 0.02f || Mathf.Abs(pos - posEnd) < 0.02f)
                {
                    LiftBody.velocity = Vector3.ClampMagnitude(LiftBody.velocity, 0f);
                    if (Mathf.Abs(pos - posStart) < 0.02f) mode = true;
                    else                                   mode = false;
                }

                if (mode) LiftBody.AddForce(0,  force, 0);
                else      LiftBody.AddForce(0, -force, 0);
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
}
