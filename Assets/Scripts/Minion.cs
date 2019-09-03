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

    private float _fitness;
    
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Slider _lifeBar;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    #endregion

    #region Properties

    public float Fitness
    {
        get => _fitness;
        set => _fitness = value;
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
        _data.LifePoints -= (damage * (float)_data.DefPoints / 100);
        _lifeBar.value = Mathf.Max(0, _data.LifePoints);

        Debug.Log(damage);
        
        if (_data.LifePoints <= 0)
        {
            OnDie();
        }

    }

    
    private void OnDie()
    {
        _fitness = GenerateFitness();
        navMeshAgent.speed = 0;
        Debug.Log(_fitness);
        gameObject.tag = "DeadMinion";
        PopulationController.Instance.SaveMinion(_data, _fitness);
    }

    private void OnFinish()
    {
        _fitness = GenerateFitness();
        navMeshAgent.speed = 0;
        Debug.Log(_fitness);
        gameObject.tag = "EndPoint";
        PopulationController.Instance.SaveMinion(_data, _fitness);
    }

    private float GenerateFitness()
    {
        float pFitness = _entireDistance - navMeshAgent.remainingDistance;
        pFitness += _data.LifePoints * _data.LifePoints;
        return pFitness;
    }
    
}