using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    void Execute();

    void AddMinion(Minion nMinion, ParticleSystem nParticle);

}
