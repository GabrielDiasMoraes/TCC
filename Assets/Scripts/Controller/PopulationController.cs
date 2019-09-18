using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Dictionary<MinionData, FitnessData> _minionsAfterWave;

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
            _minionsAfterWave = new Dictionary<MinionData, FitnessData>();
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

            tempMinion.InitialLife = tempMinion.LifePoints = GenerateRandomLife();

            tempMinion.LifeBar.value = tempMinion.LifeBar.maxValue = tempMinion.LifePoints;

            tempMinion.EntireDistance = tempAgent.remainingDistance;

            tempMinion.MinionColor = GenerateRandomColor();

            tempMinion.InitialTime = Time.time;
            
            _aliveMinions.Add(_minionsToAwake[0]);
            
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

    public void SaveMinion(MinionData pData, FitnessData pFitnessData)
    {
        _minionsAfterWave[pData] = pFitnessData;
    }

    public void CalculateFitness(int type)
    {
        switch (type)
        {
            // Only the best in distance and life remaining
            case 0:
            {
                foreach (var pMinion in _minionsAfterWave)
                {
                    pMinion.Key.Fitness = pMinion.Value.TraveledDistance + pMinion.Value.Life;
                }
                break;
            }

            // The Faster
            case 1:
            {
                float pBestSpeed = _minionsAfterWave.First().Value.Speed;
                foreach (var pMinion in _minionsAfterWave)
                {
                    if (pMinion.Value.Speed > pBestSpeed)
                    {
                        pBestSpeed = pMinion.Value.Speed;
                    }
                }

                foreach (var pMinion in _minionsAfterWave)
                {
                    pMinion.Key.Fitness = pMinion.Value.Speed/pBestSpeed;
                }
                break;
            }

            // The "Tanker"
            case 2:
            {
                float pBestMitigatedDamage = _minionsAfterWave.First().Value.MitigatedDamage;
                foreach (var pMinion in _minionsAfterWave)
                {
                    if (pMinion.Value.MitigatedDamage > pBestMitigatedDamage)
                    {
                        pBestMitigatedDamage = pMinion.Value.MitigatedDamage;
                    }
                }

                foreach (var pMinion in _minionsAfterWave)
                {
                    pMinion.Key.Fitness = pMinion.Value.MitigatedDamage/pBestMitigatedDamage;
                }
                break;
            }

            // The Smarter
            case 3:
            {
                float pBestTime = _minionsAfterWave.First().Value.TimeToFinish;
                foreach (var pMinion in _minionsAfterWave)
                {
                    if (pMinion.Value.TimeToFinish < pBestTime)
                    {
                        // The best time is the smaller number
                        pBestTime = (1.0f/pMinion.Value.TimeToFinish) * 10;
                    }
                }

                foreach (var pMinion in _minionsAfterWave)
                {
                    pMinion.Key.Fitness = pMinion.Value.TimeToFinish/pBestTime;
                }
                break;
            }
            
            // The Smarter/Tanker
            case 4:
            {
                float pBestTime = _minionsAfterWave.First().Value.TimeToFinish;
                float pBestMitigatedDamage = _minionsAfterWave.First().Value.MitigatedDamage;
                foreach (var pMinion in _minionsAfterWave)
                {
                    if (pMinion.Value.TimeToFinish < pBestTime)
                    {
                        // The best time is the smaller number
                        pBestTime = (1.0f/pMinion.Value.TimeToFinish) * 10;
                    }
                    
                    if (pMinion.Value.MitigatedDamage > pBestMitigatedDamage)
                    {
                        pBestMitigatedDamage = pMinion.Value.MitigatedDamage;
                    }
                }

                foreach (var pMinion in _minionsAfterWave)
                {
                    pMinion.Key.Fitness = (pMinion.Value.TimeToFinish/pBestTime * 3) + (pMinion.Value.MitigatedDamage/pBestMitigatedDamage * 1);
                }
                break;
            }
            
            
            // The Smarter/Faster
            case 5:
            {
                float pBestTime = _minionsAfterWave.First().Value.TimeToFinish;
                float pBestSpeed = _minionsAfterWave.First().Value.Speed;
                foreach (var pMinion in _minionsAfterWave)
                {
                    if (pMinion.Value.TimeToFinish < pBestTime)
                    {
                        // The best time is the smaller number
                        pBestTime = (1.0f/pMinion.Value.TimeToFinish) * 10;
                    }
                    
                    if (pMinion.Value.Speed > pBestSpeed)
                    {
                        pBestSpeed = pMinion.Value.Speed;
                    }
                }

                foreach (var pMinion in _minionsAfterWave)
                {
                    pMinion.Key.Fitness = (pMinion.Value.TimeToFinish/pBestTime * 3) + (pMinion.Value.Speed/pBestSpeed * 1);
                }
                break;
            }
            
        }
        
    }
}
