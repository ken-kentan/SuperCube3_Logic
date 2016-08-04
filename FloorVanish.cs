using UnityEngine;
using System.Collections;

public class FloorVanish : MonoBehaviour {

    private BoxCollider colliderFloor;
    private bool isStartVanish;
    private int alpha;

	// Use this for initialization
	void Start () {
        World.floorVanishList.Add(this);

        colliderFloor = GetComponent<BoxCollider>();
        isStartVanish = false;

        alpha = 150;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (World.isPause || !isStartVanish) return;

        if (alpha-- <= 0) colliderFloor.enabled = false;

        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, alpha/255.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && collision.transform.position.y > transform.position.y) isStartVanish = true;
    }

    public void Reset()
    {
        alpha = 150;

        colliderFloor.enabled = true;
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, alpha/255.0f);
        isStartVanish = false;
    }
}
