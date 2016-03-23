using UnityEngine;
using System.Collections;

public class Climb : MonoBehaviour
{

    public bool isCube;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (World.isPause) return;

        if (isCube)
        {
            CubeManager.cubeBody.drag = 5;
            CubeManager.ResetJump();
        }
        else
        {
            CubeManager.cubeBody.drag = 0.5f;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Cube") isCube = true;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Cube") isCube = false;
    }
}