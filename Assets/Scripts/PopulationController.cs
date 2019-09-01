using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PopulationController : MonoBehaviour
{
    [SerializeField] private GameObject _minionPrefab;

    [SerializeField] private int _minionsQuantity;

    [SerializeField] private float _mutationPercent;

    [SerializeField] private float minimumSpeed;

    [SerializeField] private float maximumSpeed;

    [SerializeField] private int minimumLife;
    
    [SerializeField] private int maximumLife;
    
    [SerializeField] private int minimumDefense;
    
    [SerializeField] private int maximumDefense;

    private Dictionary<MinionData, float> _endMinions;
    
    private Transform initPoint;

    private Transform endPoint;

    private float passedTime;

    private int minionsCreated;
    
    private void Start()
    {
        initPoint = GameObject.FindWithTag("InitPoint").transform;
        endPoint =  GameObject.FindWithTag("EndPoint").transform;
        passedTime = 0;
        minionsCreated = 0;
    }

    private void Update()
    {
        passedTime += Time.deltaTime;

        if (passedTime >= 2 && minionsCreated < _minionsQuantity)
        {
            passedTime = 0;
            minionsCreated++;
            CreateMinion();
        }
    }


    private void CreateMinion()
    {
        Minion tempMinion = Instantiate(_minionPrefab, initPoint.position, Quaternion.identity).GetComponent<Minion>();
        
        NavMeshAgent tempAgent = tempMinion.GetComponent<NavMeshAgent>();
        
        tempAgent.destination = endPoint.position;

        tempAgent.speed = tempMinion.SpeedValue = GenerateRandomSpeed();

        tempMinion.DefPoints = GenerateRandomDefense();

        tempMinion.LifePoints = GenerateRandomLife();

        tempMinion.LifeBar.value = tempMinion.LifeBar.maxValue = tempMinion.LifePoints;

        tempMinion.EntireDistance = tempAgent.remainingDistance;

        tempMinion.PopulationController = this;

        tempMinion.MinionColor = GenerateRandomColor();


    }


    private float GenerateRandomSpeed()
    {
        return Random.Range(minimumSpeed, maximumSpeed);
    }

    private int GenerateRandomDefense()
    {
        return Random.Range(minimumDefense, maximumDefense);
    }

    private int GenerateRandomLife()
    {
        return Random.Range(minimumLife, maximumLife);
    }

    // Todo
    private Color GenerateRandomColor()
    {
        return Color.red;
    }

    public void SaveMinion(MinionData pData, float pFitness)
    {
        _endMinions[pData] = pFitness;
    }
}
