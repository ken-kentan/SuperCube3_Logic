using UnityEngine;
using System.Collections;

public class CubeEffects : MonoBehaviour
{

    public Renderer rendererCube;
    public Light lightCube;
    public Material materialCube;
    public ParticleSystem particleDead;

    public static int cntTimer, cntTimerAqua, cntTimerMagnet;

    // Use this for initialization
    void Start()
    {
        cntTimer = cntTimerAqua = cntTimerMagnet = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (World.isPause) return;

        //Aqua
        if (CubeManager.effectAqua > 0)
        {
            if (++CubeManager.effectAqua >= 10)
            {
                cntTimerAqua++;
                CubeManager.effectAqua = 1;
            }

            if (cntTimerAqua % 2 == 0)
            {
                rendererCube.material = World.materialAqua;
                lightCube.color = World.colorAqua;
            }
            else {
                rendererCube.material = materialCube;
                lightCube.color = Color.white;
            }

            if (cntTimerAqua > 2) CubeManager.effectAqua = 0;
        }

        //Magnet
        if(CubeManager.effectMagnet > 0)
        {
            if(++CubeManager.effectMagnet > 1100)
            {
                CubeManager.effectMagnet = 0;
                rendererCube.material = materialCube;
                lightCube.color = Color.white;
            }
            else
            {
                if (CubeManager.effectMagnet % 15 == 0) cntTimerMagnet++;
                
                if(CubeManager.effectMagnet < 1000)
                {
                    rendererCube.material = World.materialMagnet;
                }
                else if (cntTimerMagnet % 2 == 1)
                {
                    rendererCube.material = World.materialMagnet;
                    lightCube.color = World.colorMagnet;
                }
                else {
                    rendererCube.material = materialCube;
                    lightCube.color = Color.white;
                }
            }
        }

        //Dead
        if (CubeManager.isResetCube)
        {
            particleDead.time = 0.0f;
            particleDead.Play();
        }

    }
}