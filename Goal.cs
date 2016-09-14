using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public GameObject Flag;
    public Light lightGoal;
    public static bool isEnterCube;

    private float R = 1.0f, G = 0.0f, B = 0.0f;
    private int mode = 0;

    void Awake()
    {
        isEnterCube = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (World.isPause) return;

        Flag.GetComponent<Renderer>().material.color = new Color(R, G, B);
        lightGoal.color = new Color(R, G, B);

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube") isEnterCube = true;
    }
}
