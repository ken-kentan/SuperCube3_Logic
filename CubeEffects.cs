using UnityEngine;
using System.Collections;

public class CubeEffects : MonoBehaviour
{

    public Renderer rendererCube;
    public Light lightCube;
    public Material materialCube;
    public ParticleSystem particleDead;

    public static CubeEffects Run;
    public static int cntTimer, cntTimerAqua, cntTimerMagnet;

    void Awake()
    {
        Run = this;
    }

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

                return;
            }
            else {
                ResetEffect();
            }

            if (cntTimerAqua > 2) CubeManager.effectAqua = 0;
        }

        //Magnet
        if(CubeManager.effectMagnet > 0)
        {
            if(++CubeManager.effectMagnet > 1100)
            {
                CubeManager.effectMagnet = 0;
                ResetEffect();
            }
            else
            {
                if (CubeManager.effectMagnet % 15 == 0) cntTimerMagnet++;
                
                if(CubeManager.effectMagnet < 1000)
                {
                    rendererCube.material = World.materialMagnet;
                    lightCube.color = World.colorMagnet;
                }
                else if (cntTimerMagnet % 2 == 1)
                {
                    rendererCube.material = World.materialMagnet;
                    lightCube.color = World.colorMagnet;
                }
                else {
                    ResetEffect();
                }
            }
        }

        //PlusJump
        if(CubeManager.effectPlusJump > 0)
        {
            rendererCube.material = World.materialPlusJump;
            lightCube.color = Color.green;

            if (CubeManager.isResetCube)
            {
                CubeManager.effectPlusJump = 0;
                CubeManager.maxJump = 2;
                ResetEffect();
            }
        }
    }

    void ResetEffect()
    {
        rendererCube.material = materialCube;
        lightCube.color = Color.white;
    }

    void ResetAllTimers()
    {
        cntTimer = cntTimerAqua = cntTimerMagnet = 0;
    }

    public void KillAllEffects()
    {
        CubeManager.effectMagnet = CubeManager.effectAqua = CubeManager.effectPlusJump = 0;
        CubeManager.maxJump = 2;
        ResetAllTimers();
        ResetEffect();
    }

    public void Dead()
    {
        particleDead.time = 0.0f;
        particleDead.Play();
    }
}