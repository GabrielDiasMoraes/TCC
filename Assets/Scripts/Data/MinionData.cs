using System.Collections.Generic;
using UnityEngine;

public class MinionData 
{
    #region Member Variables

    private float _lifePoints;
    private float _fullLife;
    private int _defPoints;
    private float _mitigatedDamage;
    private float _speedValue;
    private float _fitness;
    private List<AbilityTypes> _abilities;
    private Color _color;
    #endregion
    
    #region Properties
    
    public float LifePoints
    {
        get => _lifePoints;
        set => _lifePoints = value;
    }

    public float FullLife
    {
        get => _fullLife;
        set => _fullLife = value;
    }

    public float MitigatedDamage
    {
        get => _mitigatedDamage;
        set => _mitigatedDamage = value;
    }

    public int DefPoints
    {
        get => _defPoints;
        set => _defPoints = value;
    }

    public float SpeedValue
    {
        get => _speedValue;
        set => _speedValue = value;
    }

    public List<AbilityTypes> Abilities
    {
        get => _abilities;
        set => _abilities = value;
    }

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public float Fitness
    {
        get => _fitness;
        set => _fitness = value;
    }
    
    #endregion
}
