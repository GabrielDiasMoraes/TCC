using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FitnessData
{    
    
    #region MemberVariable
    
    private float _life;
    private float _traveledDistance;
    private float _speed;
    private float _originalLife;
    private float _mitigatedDamage;
    private float _intelligence;

    #endregion
    
    #region Properties
    
    public float Life
    {
        get => _life;
        set => _life = value;
    }

    public float TraveledDistance
    {
        get => _traveledDistance;
        set => _traveledDistance = value;
    }
    
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float OriginalLife
    {
        get => _originalLife;
        set => _originalLife = value;
    }

    public float MitigatedDamage
    {
        get => _mitigatedDamage;
        set => _mitigatedDamage = value;
    }

    public float Intelligence
    {
        get => _intelligence;
        set => _intelligence = value;
    }
    
    #endregion
    

}
