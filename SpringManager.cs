using UnityEngine;
using System.Collections;

public class SpringManager : MonoBehaviour {

    public float force;

    private GameObject Spring;
    private bool isOnCube, isSpring, isDeny;

    // Use this for initialization
    void Start () {
	    Spring = transform.FindChild("Spring").gameObject;

        if (force == 0) force = 300.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

        if (isOnCube && Vector3.Distance(CubeManager.pos, Spring.transform.position + new Vector3(0, 0.5f, 0)) < 0.4f)
        {
            if(!isDeny) isSpring = true;
//            UnityEngine.Debug.Log(Vector3.Distance(CubeManager.pos, Spring.transform.position + new Vector3(0, 0.5f, 0)));
        }

        if (isSpring)
        {
            isSpring = false;
            isDeny = true;
            CubeManager.StopCube();
            CubeManager.cubeBody.AddForce(0, force, 0);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube") isOnCube = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Cube")
        {
            isOnCube = isDeny = false;
        }
    }
}
