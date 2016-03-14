using UnityEngine;
using System.Collections;

public class StarManager : MonoBehaviour {

    public GameObject thisStar;
    public ParticleSystem particleStar;

    private Renderer renderStar;
    private float R, G, B;
    private int mode;

    // Use this for initialization
    void Start () {
        renderStar = GetComponent<Renderer>();

        R = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

        if (Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)
        {
            renderStar.enabled = false;
            return;
        }

        renderStar.enabled = true;
        transform.Rotate(1, 1, 1);

        thisStar.GetComponent<Renderer>().material.color = new Color(R, G, B);
        particleStar.startColor = new Color(R, G, B, 0.5f);

        switch (mode)
        {
            case 0:
                G += 0.01f;
                if (G >= 1)
                {
                    G = 1;
                    mode = 1;
                }
                break;
            case 1:
                R -= 0.01f;
                if (R <= 0)
                {
                    R = 0;
                    mode = 2;
                }
                break;
            case 2:
                B += 0.01f;
                if (B >= 1)
                {
                    B = 1;
                    mode = 3;
                }
                break;
            case 3:
                G -= 0.01f;
                if (G <= 0)
                {
                    G = 0;
                    mode = 4;
                }
                break;
            case 4:
                R += 0.01f;
                if (R >= 1)
                {
                    R = 1;
                    mode = 5;
                }
                break;
            case 5:
                B -= 0.01f;
                if (B <= 0)
                {
                    B = 0;
                    mode = 0;
                }
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube")
        {
            World.audioSource.PlayOneShot(World.getMagnetSE);
            CubeEffects.Run.KillAllEffects();
            CubeManager.effectMagnet = 1;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
