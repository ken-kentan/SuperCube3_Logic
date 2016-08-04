using UnityEngine;
using System.Collections;

public class AquaManager : MonoBehaviour {
    
    private Renderer renderAqua;

	// Use this for initialization
	void Start () {
        renderAqua = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (World.isPause) return;

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
            World.sumAqua++;
            GameDataManager.AddDataValue(GameDataManager.Data.Aqua);
            World.effect.Enable(CubeEffects.Effect.Aqua);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
