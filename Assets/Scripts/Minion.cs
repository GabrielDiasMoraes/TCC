using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Minion : MonoBehaviour
{
    #region Member Variables

    private float _lifePoints;
    private int _defPoints;
    private float _speedValue;
    private List<AbilityTypes> _abilities;

    private float _entireDistance;

    private float _fitness;
    
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Slider _lifeBar;
    
    #endregion

    #region Properties

    public float Fitness
    {
        get => _fitness;
        set => _fitness = value;
    }

    public bool isAlive => _lifePoints > 0;
    
    public Slider LifeBar => _lifeBar;

    public float EntireDistance
    {
        get => _entireDistance;
        set => _entireDistance = value;
    }

    public float LifePoints
    {
        get => _lifePoints;
        set => _lifePoints = value;
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

    #endregion
    

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    public void TakeDamage(float damage)
    {
        _lifePoints -= (damage * (float)_defPoints / 100);
        _lifeBar.value = Mathf.Max(0, _lifePoints);

        Debug.Log(damage);
        
        if (_lifePoints <= 0)
        {
            OnDie();
        }

    }

    
    private void OnDie()
    {
        _fitness = navMeshAgent.remainingDistance;
        navMeshAgent.speed = 0;
        Debug.Log(_fitness);
        gameObject.tag = "DeadMinion";
    }
}