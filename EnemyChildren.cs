using UnityEngine;
using System.Collections;

public class EnemyChildren : MonoBehaviour {

    public GameObject EnemyChieldren;
    private Rigidbody EnemyBody;

	// Use this for initialization
	void Start () {
        EnemyBody = GetComponent<Rigidbody>();

        Vector3 forceDirection = World.Cube.transform.localPosition - transform.localPosition;
        float sum = (Mathf.Abs(forceDirection.x) + Mathf.Abs(forceDirection.y));

        float rate = 500 / sum;

        EnemyBody.AddForce(forceDirection * rate);
    }
	
	// Update is called once per frame
	void Update () {
        if (World.pause) return;

        if (Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance || CubeManager.isResetCube) Destroy(EnemyChieldren);
	}
}
