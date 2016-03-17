using UnityEngine;
using System.Collections;

public class MagnetManager : MonoBehaviour {
    
    private Renderer renderMagnet;

    // Use this for initialization
    void Start () {
        renderMagnet = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

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
            World.audioSource.PlayOneShot(World.getMagnetSE);
            CubeEffects.Run.KillAllEffects();
            CubeManager.effectMagnet = 1;
            World.sumMagnet++;
            GameDataManager.AddDataValue(GameDataManager.Data.Magnet);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
