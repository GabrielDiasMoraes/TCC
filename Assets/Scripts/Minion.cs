using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Minion : MonoBehaviour
{
    #region Member Variables

    private MinionData _data;
    
    private float _entireDistance;

    private FitnessData _fitnessData;
   
    private NavMeshAgent navMeshAgent;

    private float _initialTime;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Slider _lifeBar;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    #endregion

    #region Properties

    public FitnessData FitnessData
    {
        get => _fitnessData;
        set => _fitnessData = value;
    }

    public float InitialLife
    {
        get => _data.FullLife;
        set => _data.FullLife = value;
    }

    public float InitialTime
    {
        get => _initialTime;
        set => _initialTime = value;
    }
    
    public MinionData Data
    {
        get => _data;
        set => _data = value;
    }
    
    public bool isAlive => _data.LifePoints > 0;
    
    public Slider LifeBar => _lifeBar;

    public float EntireDistance
    {
        get => _entireDistance;
        set => _entireDistance = value;
    }

    public Color MinionColor
    {
        get => _data.Color;
        set
        {
            _data.Color = value;
            _spriteRenderer.color = value;
        }
    }

    public float LifePoints
    {
        get => _data.LifePoints;
        set => _data.LifePoints = value;
    }

    public int DefPoints
    {
        get => _data.DefPoints;
        set => _data.DefPoints = value;
    }

    public float SpeedValue
    {
        get => _data.SpeedValue;
        set => _data.SpeedValue = value;
    }

    public List<AbilityTypes> Abilities
    {
        get => _data.Abilities;
        set => _data.Abilities = value;
    }

    #endregion
    

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void LateUpdate()
    {
        _animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    public void TakeDamage(float damage)
    {
        float tEndDamage = (damage * (float)_data.DefPoints / 100);
        _data.LifePoints -= tEndDamage;
        _lifeBar.value = Mathf.Max(0, _data.LifePoints);
        _data.MitigatedDamage += damage - tEndDamage;

        Debug.Log(damage);
        
        if (_data.LifePoints <= 0)
        {
            OnDie();
        }

    }

    
    private void OnDie()
    {
        _fitnessData = GenerateFitness();
        navMeshAgent.speed = 0;
        Debug.Log(_fitnessData);
        gameObject.tag = "DeadMinion";
        PopulationController.Instance.SaveMinion(_data, _fitnessData);
    }

    private void OnFinish()
    {
        _fitnessData = GenerateFitness();
        navMeshAgent.speed = 0;
        Debug.Log(_fitnessData);
        gameObject.tag = "EndPoint";
        PopulationController.Instance.SaveMinion(_data, _fitnessData);
    }

    private FitnessData GenerateFitness()
    {
        FitnessData fitnessData = new FitnessData();
        fitnessData.TraveledDistance = _entireDistance - navMeshAgent.remainingDistance;
        fitnessData.Life = _data.LifePoints;
        fitnessData.Speed = _data.SpeedValue;
        fitnessData.OriginalLife = _data.FullLife;
        fitnessData.MitigatedDamage = _data.MitigatedDamage;
        fitnessData.TimeToFinish = Time.time - _initialTime; 
        return fitnessData;
    }
    
}