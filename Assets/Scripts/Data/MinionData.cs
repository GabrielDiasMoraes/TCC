using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MinionData 
{
    #region Member Variables

    private float _lifePoints;
    private float _fullLife; //
    private float _defPoints; //
    private float _mitigatedDamage;
    private float _speedValue; //
    private float _intelligence; //
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

    public float DefPoints
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

    public float Intelligence
    {
        get => _intelligence;
        set => _intelligence = value;
    }
    
    #endregion

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder("");
        builder.Append("FullLife: " + _fullLife);
        builder.Append("\nSpeed: " + _speedValue);
        builder.Append("\nArmor: " + _defPoints);
        builder.Append("\nIntelligence: " + _intelligence);
        return builder.ToString();
    }

    public MinionData()
    {
        
    }
    
    public MinionData(MinionData pMinionData)
    {
        _lifePoints = pMinionData.LifePoints;
        _fullLife = pMinionData.FullLife; 
        _defPoints = pMinionData.DefPoints; 
        _mitigatedDamage = pMinionData.MitigatedDamage;
        _speedValue = pMinionData.SpeedValue; 
        _intelligence = pMinionData.Intelligence; 
        _fitness = pMinionData.Fitness;
        _abilities = pMinionData.Abilities;
        _color = pMinionData.Color;
    }
}
