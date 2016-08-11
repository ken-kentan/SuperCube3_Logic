using UnityEngine;
using System.Collections;

public class CubeEffects : MonoBehaviour
{
    public enum Effect { Aqua, Magnet, PlusJump};

    public Renderer rendererCube;
    public Light lightCube;
    public ParticleSystem particleDead;
    public Effect effect;
    
    public static bool isAqua, isMagnet, isPlusJump;
    public static int cntTimerAqua, cntTimerMagnet;
    public static int cntFlashAqua, cntFlashManget;


    // Use this for initialization
    void Start()
    {
        World.effect = this;

        cntTimerAqua = cntTimerMagnet = 0;
        cntFlashAqua = cntFlashManget = 0;
        isAqua = isMagnet = isPlusJump = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (World.isPause || CubeManager.isMotionDead) return;

        //Magnet
        if(isMagnet)
        {
            if(++cntTimerMagnet > 1100)
            {
                isMagnet = false;
                Reset();
            }
            else
            {
                if (cntTimerMagnet < 1000)
                {
                    rendererCube.material = World.materialMagnet;
                    lightCube.color = World.colorMagnet;
                }
                else if (cntTimerMagnet % 10 == 0) cntFlashManget++;

                if (cntFlashManget % 2 == 0)
                {
                    rendererCube.material = World.materialMagnet;
                    lightCube.color = World.colorMagnet;
                }
                else {
                    Reset();
                }
            }
        }

        //PlusJump
        if(isPlusJump)
        {
            rendererCube.material = World.materialPlusJump;
            lightCube.color = Color.green;
        }

        //Aqua
        if (isAqua)
        {
            if (++cntTimerAqua >= 10)
            {
                cntFlashAqua++;
                cntTimerAqua = 0;
            }

            if (cntFlashAqua % 2 == 0)
            {
                rendererCube.material = World.materialAqua;
                lightCube.color = World.colorAqua;

                return;
            }
            else {
                if(!isMagnet && !isPlusJump) Reset();
            }

            if (cntFlashAqua > 2) isAqua = false;
        }
    }

    public void Enable(Effect type)
    {
        switch (type)
        {
            case Effect.Aqua:
                isAqua = true;
                cntFlashAqua = cntTimerAqua = 0;
                break;
            case Effect.Magnet:
                isMagnet = true;
                cntFlashManget = cntTimerMagnet = 0;
                break;
            case Effect.PlusJump:
                isPlusJump = true;
                break;
        }
    }

    public void Reset()
    {
        isPlusJump = false;
        CubeManager.maxJump = 2;
        rendererCube.material = World.materialCube;
        lightCube.color = Color.white;
    }

    void ResetAllTimers()
    {
        cntTimerAqua = cntTimerMagnet = 0;
    }

    public void KillAllEffects()
    {
        isAqua = isMagnet = isPlusJump = false;
        CubeManager.maxJump = 2;
        ResetAllTimers();
        Reset();
    }

    public void Dead()
    {
        particleDead.time = 0.0f;
        particleDead.Play();

        rendererCube.material = World.materialBlack;
        lightCube.color = Color.black;
    }
}