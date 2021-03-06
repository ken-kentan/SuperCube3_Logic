﻿using UnityEngine;
using System.Collections;

public class MagnetManager : MonoBehaviour {
    
    private Renderer renderMagnet;

    // Use this for initialization
    void Start () {
        renderMagnet = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
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
            World.effect.KillAllEffects();
            World.effect.Enable(CubeEffects.Effect.Magnet);
            World.sumMagnet++;
            GameDataManager.AddDataValue(GameDataManager.Data.Magnet);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
