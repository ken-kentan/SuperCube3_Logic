using UnityEngine;
using System.Collections;

public class PlusJumpManager : MonoBehaviour {

    private Renderer renderJump;

    // Use this for initialization
    void Start()
    {
        renderJump = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (World.isPause) return;

        if (Vector3.Distance(World.Cube.transform.position, transform.position) > World.drawDistance)
        {
            renderJump.enabled = false;
            return;
        }

        renderJump.enabled = true;
        transform.Rotate(1, 1, 1);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Cube")
        {
            World.audioSource.PlayOneShot(World.getJumpSE);
            CubeEffects.Run.KillAllEffects();
            CubeManager.maxJump = 3;
            CubeManager.effectPlusJump = 1;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
