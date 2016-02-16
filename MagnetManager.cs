using UnityEngine;
using System.Collections;

public class MagnetManager : MonoBehaviour {
    
    public AudioClip getMagnetSE;
    private Renderer renderMagnet;

    // Use this for initialization
    void Start () {
        renderMagnet = GetComponent<Renderer>();

        if (World.materialMagnet == null) World.materialMagnet = renderMagnet.material;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)
        {
            renderMagnet.enabled = false;
            return;
        }

        renderMagnet.enabled = true;
        transform.Rotate(1, 1, 1);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube")
        {
            World.audioSource.PlayOneShot(getMagnetSE);
            CubeManager.effectMagnet = 1;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
