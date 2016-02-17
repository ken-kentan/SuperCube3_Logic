using UnityEngine;
using System.Collections;

public class AquaManager : MonoBehaviour {
    
    private Renderer renderAqua;

	// Use this for initialization
	void Start () {
        renderAqua = GetComponent<Renderer>();

        if (World.materialAqua == null) World.materialAqua = renderAqua.material;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)
        {
            renderAqua.enabled = false;
            return;
        }

        renderAqua.enabled = true;
        transform.Rotate(1, 1, 1);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube")
        {
            World.audioSource.PlayOneShot(World.getAquaSE);
            CubeManager.life++;
            CubeManager.effectAqua = 1;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
