﻿using UnityEngine;
using System.Collections;

public class BlockSecret : MonoBehaviour {

    public GameObject objectsSecret;

    private Collider thisCollider;
    private float posY;

    // Use this for initialization
    void Start () {
        thisCollider = GetComponent<Collider>();
        posY = transform.position.y - 1;
    }
	
	// Update is called once per frame
	void Update () {
	    if(CubeManager.posY > posY) thisCollider.isTrigger = true;
        else                        thisCollider.isTrigger = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Cube") return;

        GetComponent<Renderer>().material = World.materialBlockSecret;

        if (objectsSecret != null) objectsSecret.SetActive(true);

        GameDataManager.SecretBlock++;
        GameDataManager.SaveSecret();

        Destroy(this);
    }
}
