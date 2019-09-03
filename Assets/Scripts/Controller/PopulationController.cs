using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PopulationController : MonoBehaviour
{
    
    public static PopulationController Instance { get; private set; }
    
    [SerializeField] private GameObject _minionPrefab;

    [SerializeField] public int _minionsQuantity;

    [SerializeField] public float _mutationPercent;

    [SerializeField] public float minimumSpeed;

    [SerializeField] public float maximumSpeed;

    [SerializeField] public int minimumLife;
    
    [SerializeField] public int maximumLife;
    
    [SerializeField] public int minimumDefense;
    
    [SerializeField] public int maximumDefense;

    public Dictionary<MinionData, float> _minionsAfterWave;

    public List<GameObject> _minionsToAwake;

    public List<GameObject> _aliveMinions;
    
    public Transform initPoint;

    public Transform endPoint;

    private float passedTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        initPoint = GameObject.FindWithTag("InitPoint").transform;
        endPoint =  GameObject.FindWithTag("EndPoint").transform;

        Initialize();

    }

    private void Initialize()
    {
        if (_aliveMinions == null)
        {
            _aliveMinions = new List<GameObject>();
        }

        if (_minionsToAwake == null)
        {
            _minionsToAwake = new List<GameObject>();
        }

        if (_minionsAfterWave == null)
        {
            _minionsAfterWave = new Dictionary<MinionData, float>();
        }

        for (int i = _aliveMinions.Count - 1; i >= 0; i--)
        {
            Destroy(_aliveMinions[i]);
            _aliveMinions.RemoveAt(i);
        }
        
        for (int i = _minionsToAwake.Count - 1; i >= 0; i--)
        {
            Destroy(_minionsToAwake[i]);
            _minionsToAwake.RemoveAt(i);
        }

        _minionsAfterWave.Clear();

        passedTime = 0;
        
        for (int i = 0; i < _minionsQuantity; i++)
        {
            _minionsToAwake.Add(CreateMinion());
        }
    }

    private void Update()
    {
        passedTime += Time.deltaTime;

        if (passedTime >= 2)
        {
            passedTime = 0;
            AwakeMinion();
        }
    }


    private GameObject CreateMinion()
    {
        GameObject temp = Instantiate(_minionPrefab, initPoint.position, Quaternion.identity);
        
        temp.SetActive(false);

        return temp;
    }

    private void AwakeMinion()
    {
        if (_minionsToAwake.Count > 0)
        {
            _minionsToAwake[0].SetActive(true);

            Minion tempMinion = _minionsToAwake[0].GetComponent<Minion>();
            
            tempMinion.Data = new MinionData();
        
            NavMeshAgent tempAgent = tempMinion.GetComponent<NavMeshAgent>();
        
            tempAgent.destination = endPoint.position;

            tempAgent.speed = tempMinion.SpeedValue = GenerateRandomSpeed();
        
            tempMinion.DefPoints = GenerateRandomDefense();

            tempMinion.LifePoints = GenerateRandomLife();

            tempMinion.LifeBar.value = tempMinion.LifeBar.maxValue = tempMinion.LifePoints;

            tempMinion.EntireDistance = tempAgent.remainingDistance;

            tempMinion.MinionColor = GenerateRandomColor();
            
            
            _minionsToAwake.RemoveAt(0);
        }
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
        float red = Random.value;
        float green = Random.value;
        float blue = Random.value;
        return new Color(red, green, blue);
    }

    public void SaveMinion(MinionData pData, float pFitness)
    {
        _minionsAfterWave[pData] = pFitness;
    }
}
