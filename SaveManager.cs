using UnityEngine;
using System.Collections;

public class SaveManager : MonoBehaviour {

    private Renderer rendererSave;
    private int cntTimer, cntColorChange;

    // Use this for initialization
    void Start() {
        rendererSave = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {
        if (cntTimer == 0 || cntColorChange > 2) return;

        if (cntTimer++ % 10 == 0)
        {
            cntColorChange++;
            cntTimer = 1;
        }

        if (cntColorChange % 2 == 0) rendererSave.material.color = Color.white;
        else                         rendererSave.material.color = new Color(0, 1, 0);
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && cntTimer == 0)
        {
            World.audioSource.PlayOneShot(World.saveSE);
            cntTimer = 1;
            World.posReborn = new Vector3(transform.localPosition.x, transform.localPosition.y + 1.5f, 0);

            GameDataManager.AddDataValue(GameDataManager.Data.Save);

            Destroy(this, 5f);
        }
    }
}
