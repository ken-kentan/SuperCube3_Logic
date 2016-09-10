using UnityEngine;
using System.Collections;

public class Climb : MonoBehaviour
{
    private bool isCube;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (World.isPause) return;
        
        if (isCube) World.cubeManager.ResetJump();
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Cube")
        {
            isCube = true;
            World.cubeManager.cubeBody.drag = 5;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Cube")
        {
            isCube = false;
            World.cubeManager.cubeBody.drag = 0.5f;
        }
    }
}