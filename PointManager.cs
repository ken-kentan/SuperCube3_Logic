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
	void Update () {
        if (World.isPause) return;

        distanceCube = Vector3.Distance(World.Cube.transform.position, transform.position);

        if (distanceCube > World.drawDistance)
        {
            renderPoint.enabled = false;
            return;
        }

        if (CubeManager.effectMagnet > 0 && distanceCube < 11.0f) isMagnet = true;

        renderPoint.enabled = true;
        transform.Rotate(1, 1, 1);

        if (isMagnet)
        {
            float posX = transform.localPosition.x,
                  posY = transform.localPosition.y;

            posX += (CubeManager.posX - transform.localPosition.x) / 6.0f;
            posY += (CubeManager.posY - transform.localPosition.y) / 6.0f;

            transform.localPosition = new Vector3(posX, posY, 0);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube")
        {
            if (isBig)
            {
                World.sumPoint += 10;
                GameDataManager.Point += 10;
            }
            else {
                World.sumPoint++;
                GameDataManager.Point++;
            }

            World.audioSource.PlayOneShot(World.pointSE);
            GameDataManager.SaveCollection();
            this.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}