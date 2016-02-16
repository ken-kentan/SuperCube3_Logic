using UnityEngine;
using System;
using System.Collections;

public class World : MonoBehaviour {

    public static GameObject Cube;
    public static AudioSource audioSource;
    public static Color alpha = new Color(0, 0, 0, 0.01f);
    public static Material materialAqua, materialMagnet;
    public static float drawDistance;
    public static Color colorAqua   = new Color(0, 0.5f, 1, 1),
                        colorMagnet = new Color(0.6f, 1, 0, 1);

    // Use this for initialization
    void Start () {
        Cube = GameObject.Find("Cube");
        audioSource = Cube.GetComponent<AudioSource>();

        drawDistance = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
