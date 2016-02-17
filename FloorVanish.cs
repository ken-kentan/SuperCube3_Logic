using UnityEngine;
using System.Collections;

public class FloorVanish : MonoBehaviour {

    private BoxCollider colliderFloor;
    private bool isStartVanish;
    public int cnt;

	// Use this for initialization
	void Start () {
        colliderFloor = GetComponent<BoxCollider>();
        isStartVanish = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (World.pause || !isStartVanish) return;

        if (CubeManager.isResetCube) reset();

        if (cnt++ > 200) colliderFloor.enabled = false;

        float alpha = (200f - cnt) / 200f;

        if (alpha > 0.8f) alpha = 0.8f;

        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, alpha);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && collision.transform.position.y > transform.position.y) isStartVanish = true;
    }

    void reset()
    {
        colliderFloor.enabled = true;
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.8f);
        isStartVanish = false;
        cnt = 0;
    }
}
