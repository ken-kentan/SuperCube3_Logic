using UnityEngine;
using System.Collections;

public class BlockSecret : MonoBehaviour {

    public GameObject objectsSecret;

    private Collider thisCollider;

    // Use this for initialization
    void Start () {
        thisCollider = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(CubeManager.posY > transform.position.y - 1) thisCollider.isTrigger = true;
        else                                            thisCollider.isTrigger = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Renderer>().material = World.materialBlockSecret;

        if (objectsSecret != null) objectsSecret.SetActive(true);

        Destroy(this);
    }
}
