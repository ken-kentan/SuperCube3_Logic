using UnityEngine;
using System.Collections;

public class FloorSpeedManager : MonoBehaviour {
    
    public GameObject[] imgArrow = new GameObject[5];
    public float forceSpeed, animSpeed;

    private static readonly float posStart = -0.448f, posEnd = 0.448f;
    private float posY;

	// Use this for initialization
	void Start () {
        animSpeed = 0.01f * (forceSpeed/10.0f);

        posY = transform.localPosition.y - 1;

        imgArrow[0].transform.localPosition = new Vector3(posStart, 0, -0.51f);
        imgArrow[1].transform.localPosition = new Vector3(-0.2688f, 0, -0.51f);
        imgArrow[2].transform.localPosition = new Vector3(-0.0896f, 0, -0.51f);
        imgArrow[3].transform.localPosition = new Vector3( 0.0896f, 0, -0.51f);
        imgArrow[4].transform.localPosition = new Vector3( 0.2688f, 0, -0.51f);
    }
	
	// Update is called once per frame
	void Update () {

        for(int i=0; i < 5; i++)
        {
            float posArrowX = imgArrow[i].transform.localPosition.x;

            if (posArrowX > posEnd) SetArrowPosStart(imgArrow[i]);

            imgArrow[i].transform.localPosition += new Vector3(animSpeed, 0, 0);
            
            if (posArrowX >= posEnd - 0.1f) imgArrow[i].GetComponent<Renderer>().material.color = new Color(0, 0, 0, (posEnd - posArrowX) * 10);
            else if (posArrowX <= posStart + 0.1f) imgArrow[i].GetComponent<Renderer>().material.color = new Color(0, 0, 0, -(posStart - posArrowX) * 10);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && CubeManager.posY > posY)
        {
            CubeManager.cubeBody.AddForce(forceSpeed, 0, 0);
        }
    }

    void SetArrowPosStart(GameObject imgArrow)
    {
        imgArrow.transform.localPosition = new Vector3(posStart, 0, -0.51f);
        imgArrow.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
    }
}
