using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : IAbility
{

    private float fTime;

    private float activeTime;
    
    private  float randomTime;

    private  Minion pMinion;

    private ParticleSystem pParticleSystem;
    
    public Minion PMinion
    {
        get => pMinion;
        set => pMinion = value;
    }

    public ShieldAbility()
    {
        activeTime = fTime = 0f;
        randomTime = GenerateRandomTime();
    }

    public void AddMinion(Minion nMinion, ParticleSystem nParticle)
    {
        pMinion = nMinion;
        pParticleSystem = nParticle;
        ParticleSystem.MainModule settings = nParticle.main;
        settings.startColor = pMinion.MinionColor;
    }
    
    
    public void Execute()
    {
        if (pMinion.IsInvencible)
        {
            activeTime += Time.deltaTime;
            if (activeTime < 2f) return;
            pMinion.IsInvencible = false;
            if(pParticleSystem.gameObject.activeInHierarchy)
                pParticleSystem.Stop();
            activeTime = 0f;
        }
        else
        {
            fTime += Time.deltaTime;
            if (fTime < randomTime) return;
            pMinion.IsInvencible = true;
            if(pParticleSystem.gameObject.activeInHierarchy)
                pParticleSystem.Play();
            fTime = 0f;
            randomTime = GenerateRandomTime();
        }



    }

    private float GenerateRandomTime()
    {
        return Random.Range(2f, 10f);
    }

}
