using UnityEngine;
using System.Collections;

public class PointManager : MonoBehaviour {
    
    public Renderer renderPoint;
    public bool isBig;

    private float distanceCube;
    private bool isMagnet;

    // Use this for initialization
    void Start () {
        isMagnet = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (World.isPause) return;

        distanceCube = Vector3.Distance(World.Cube.transform.position, transform.position);

        if (distanceCube > World.drawDistance)
        {
            renderPoint.enabled = false;
            return;
        }

        if (CubeEffects.isMagnet && distanceCube < 11.0f) isMagnet = true;

        renderPoint.enabled = true;
        transform.Rotate(1, 1, 1);

        if (isMagnet)
        {
            Vector3 thisPos = transform.position,
                    direction = World.cubeManager.pos - thisPos;

            direction.Normalize();

            transform.position = thisPos + direction * 20.0f * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube" && !World.cubeManager.isMotionDead)
        {
            if (isBig)
            {
                World.sumPoint += 10;
                GameDataManager.AddDataValue(GameDataManager.Data.Point, 10);
            }
            else {
                World.sumPoint++;
                GameDataManager.AddDataValue(GameDataManager.Data.Point);
            }

            World.audioSource.PlayOneShot(World.pointSE);
            this.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}