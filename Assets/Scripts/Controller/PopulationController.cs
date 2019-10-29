using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using System.Linq;
using System.Text;

public class PopulationController : MonoBehaviour
{
    #region Member Variables
    public static PopulationController Instance { get; private set; }
    
    [SerializeField] private GameObject _minionPrefab;
    
    [SerializeField] private GameObject _lifeBarPrefab;

    [SerializeField] private int _minionsQuantity;

    [SerializeField] private float _mutationPercent;

    private float _elitismTopTierPercent;
    
    private float _elitismBottomTierPercent;

    [SerializeField] private float minimumSpeed;

    [SerializeField] private float maximumSpeed;

    [SerializeField] private int minimumLife;
    
    [SerializeField] private int maximumLife;
    
    [SerializeField] private int minimumDefense;
    
    [SerializeField] private int maximumDefense;

    [SerializeField] private int minimumElitismPercent;
    
    [SerializeField] private int maximumElitismPercent;

    private FitnessTypes fitnessType;

    public Dictionary<MinionData, FitnessData> _minionsAfterWave;

    public List<GameObject> _minionsToAwake;

    public List<GameObject> _aliveMinions;

    public List<MinionData> _minionsFromCrossover;
    
    public Transform initPoint;

    public Transform endPoint;

    [SerializeField] private GameObject _endTurnWindowPrefab;

    [SerializeField] private int _minionsToWinQnty;
    
    [SerializeField] private int roundsQnty;
    
    private GameObject _endTurnWindow;
    
    private float passedTime;

    private int diedMinions;
    private int reachEndMinions;
    private int roundsQntyCount;

    [SerializeField]
    private float timeBetweenSpawn;

    private float _mBestLife;
    private float _mBestSpeed;
    private float _mBestDefense;
    private float _mBestIntelligence;

    #endregion
    
    #region Properties

    public int MinionsQuantity
    {
        get => _minionsQuantity;
        set => _minionsQuantity = value;
    }

    public float MutationPercent
    {
        get => _mutationPercent;
        set => _mutationPercent = value;
    }

    public float ElitismTopTierPercent
    {
        get => _elitismTopTierPercent;
        set => _elitismTopTierPercent = value;
    }

    public float ElitismBottomTierPercent
    {
        get => _elitismBottomTierPercent;
        set => _elitismBottomTierPercent = value;
    }

    public float MinimumSpeed
    {
        get => minimumSpeed;
        set => minimumSpeed = value;
    }

    public float MaximumSpeed
    {
        get => maximumSpeed;
        set => maximumSpeed = value;
    }

    public int MinimumLife
    {
        get => minimumLife;
        set => minimumLife = value;
    }

    public int MaximumLife
    {
        get => maximumLife;
        set => maximumLife = value;
    }

    public int MinimumDefense
    {
        get => minimumDefense;
        set => minimumDefense = value;
    }

    public int MaximumDefense
    {
        get => maximumDefense;
        set => maximumDefense = value;
    }

    public int MinimumElitismPercent
    {
        get => minimumElitismPercent;
        set => minimumElitismPercent = value;
    }

    public int MaximumElitismPercent
    {
        get => maximumElitismPercent;
        set => maximumElitismPercent = value;
    }

    public Dictionary<MinionData, FitnessData> MinionsAfterWave
    {
        get => _minionsAfterWave;
        set => _minionsAfterWave = value;
    }

    public List<GameObject> MinionsToAwake
    {
        get => _minionsToAwake;
        set => _minionsToAwake = value;
    }

    public List<GameObject> AliveMinions
    {
        get => _aliveMinions;
        set => _aliveMinions = value;
    }

    public List<MinionData> MinionsFromCrossover
    {
        get => _minionsFromCrossover;
        set => _minionsFromCrossover = value;
    }

    public Transform InitPoint
    {
        get => initPoint;
        set => initPoint = value;
    }

    public Transform EndPoint
    {
        get => endPoint;
        set => endPoint = value;
    }

    public GameObject EndTurnWindow
    {
        get => _endTurnWindow;
        set => _endTurnWindow = value;
    }

    public float PassedTime
    {
        get => passedTime;
        set => passedTime = value;
    }
    
    public FitnessTypes FitnessType
    {
        get => fitnessType;
        set => fitnessType = value;
    }

    #endregion
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        Time.timeScale = 1;
    }

    private void Start()
    {
        Initialize();
        _endTurnWindow = Instantiate(_endTurnWindowPrefab);
        _endTurnWindow.SetActive(false);

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

        diedMinions = 0;
        reachEndMinions = 0;
        roundsQntyCount = 1;

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

        GameController.Instance.TurnsTextMeshProUgui.text = roundsQntyCount + "/" + roundsQnty;
        GameController.Instance.MinionsCountToWin.text = (Math.Max(_minionsToWinQnty - reachEndMinions, 0)).ToString();
    }

    private void Update()
    {
        passedTime += Time.deltaTime;

        if (passedTime >= timeBetweenSpawn)
        {
            passedTime = 0;
            AwakeMinion();
        }
    }


    private GameObject CreateMinion()
    {
        GameObject temp = Instantiate(_minionPrefab, initPoint.position, Quaternion.identity);
        GameObject lifebar = Instantiate(_lifeBarPrefab);
        temp.GetComponent<Minion>().LifeBar = lifebar.GetComponent<Lifebar>();
        temp.GetComponent<Minion>().LifeBar.TargetToFollow = temp;
        temp.GetComponent<Minion>().LifeBar.gameObject.SetActive(false);
      
        temp.SetActive(false);
        
        temp.SetActive(false);

        return temp;
    }

    private void AwakeMinion()
    {
        if (_minionsToAwake.Count > 0)
        {
            _minionsToAwake[0].SetActive(true);
            
            Minion tempMinion = _minionsToAwake[0].GetComponent<Minion>();
            
            NavMeshAgent tempAgent = tempMinion.GetComponent<NavMeshAgent>();
            
            tempMinion.LifeBar.gameObject.SetActive(true);
            
            if(!tempMinion.IsFromCrossover)
            {
                tempMinion.Data = new MinionData();
            }

            tempMinion.Destination = tempAgent.destination = endPoint.position;
            
            tempMinion.InitialTime = Time.time;
            
            tempMinion.MinionColor = GenerateRandomColor();

            tempMinion.EntireDistance = tempAgent.remainingDistance;   
            
            if (!tempMinion.IsFromCrossover)
            {
                tempAgent.speed = tempMinion.SpeedValue = GenerateRandomSpeed();

                tempMinion.DefPoints = GenerateRandomDefense();

                tempMinion.InitialLife = tempMinion.LifePoints = GenerateRandomLife();                       
            }
            else
            {
                tempAgent.speed = tempMinion.SpeedValue;
                tempMinion.LifePoints = tempMinion.InitialLife;
            }
                     
            tempMinion.LifeBar.Slider.value = tempMinion.LifeBar.Slider.maxValue = tempMinion.LifePoints;
            
            tempMinion.Started = true;
            
            _aliveMinions.Add(_minionsToAwake[0]);
            
            _minionsToAwake.RemoveAt(0);
            
            tempMinion.LifeBar.gameObject.SetActive(true);
        }
    }

    private float GenerateRandomSpeed()
    {
        return Random.Range(minimumSpeed, maximumSpeed);
    }

    private float GenerateRandomIntelligence()
    {
        return Random.Range(0, 10);
    }

    private float GenerateRandomDefense()
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

    public void SaveMinion(MinionData pData, FitnessData pFitnessData, bool bDied = false)
    {
        _minionsAfterWave[pData] = pFitnessData;
        if (_minionsAfterWave.Count >= (_minionsToAwake.Count + _aliveMinions.Count))
        {
            DisplayEndTurnSelection();
        }

        if (bDied)
        {
            diedMinions++;
        }
        else
        {
            reachEndMinions++;
            GameController.Instance.MinionsCountToWin.text = (Math.Max(_minionsToWinQnty - reachEndMinions, 0)).ToString();
        }
    }

    private void DisplayEndTurnSelection()
    {
        _endTurnWindow.SetActive(true);
    }

    public void EndTurn()
    {
        diedMinions = 0;
        reachEndMinions = 0;
        
        GameController.Instance.MinionsCountToWin.text = (Math.Max(_minionsToWinQnty - reachEndMinions, 0)).ToString();
        
        int i = 0;
        List<MinionData> _minionsAfterWaveList = SortMinions(_minionsAfterWave);
        
        _minionsAfterWave.Clear();
            
        _minionsAfterWaveList = Elitization(_minionsAfterWaveList);
            
        Crossover(_minionsAfterWaveList);
        i = 0;

        Mutation();
        i = 0;

        TransformToGameObject(_minionsFromCrossover);

        roundsQntyCount++;
        GameController.Instance.TurnsTextMeshProUgui.text = roundsQntyCount + "/" + roundsQnty;
    }

    public void TransformToGameObject(List<MinionData> pMinions)
    {
        for (int i = _aliveMinions.Count - 1; i >= 0; i--)
        {
            Destroy(_aliveMinions[i].GetComponent<Minion>().LifeBar.gameObject);
            Destroy(_aliveMinions[i].GetComponent<Minion>().DeadModel);
            Destroy(_aliveMinions[i]);
            _aliveMinions.RemoveAt(i);
        }
        
        for (int i = _minionsToAwake.Count - 1; i >= 0; i--)
        {
            Destroy(_minionsToAwake[i].GetComponent<Minion>().LifeBar.gameObject);
            Destroy(_minionsToAwake[i].GetComponent<Minion>().DeadModel);
            Destroy(_minionsToAwake[i]);
            _minionsToAwake.RemoveAt(i);
        }

        foreach (var pMinion in pMinions)
        {
            GameObject minionGameObject = CreateMinion();
            minionGameObject.GetComponent<Minion>().Data = new MinionData(pMinion);
            minionGameObject.GetComponent<Minion>().IsFromCrossover = true;
            _minionsToAwake.Add(minionGameObject);
        }
    }
    
    public void CalculateFitness()
    {
        DoCalculateFitness(fitnessType);
    }

    public void DoCalculateFitness(FitnessTypes type)
    {
        switch (type)
        {
            // Only the best in distance and life remaining
            case FitnessTypes.Distance:
            {
                DoCalculateFitnessDistance();
                break;
            }

            // The Faster
            case FitnessTypes.Speed:
            {
                DoCalculateFitnessSpeed();
                break;
            }

            // The "Tanker"
            case FitnessTypes.Defense:
            {
                DoCalculateFitnessDefense();
                break;
            }

            // The Smarter
            case FitnessTypes.Smarter:
            {
                DoCalculateFitnessIntelligence();
                break;
            }
            
            // The Smarter/Tanker
            case FitnessTypes.DefenseSmarter:
            {
                DoCalculateFitnessIntelligenceDefense();
                break;
            }
            
            
            // The Smarter/Faster
            case FitnessTypes.SpeedSmarter:
            {
                DoCalculateFitnessIntelligenceSpeed();
                break;
            }
        }
        
    }

    

    #region FitnessTypes

    private void FindBetterAttributes()
    {
        FitnessData pFirstMinion = _minionsAfterWave.First().Value;
        _mBestLife = pFirstMinion.Life;
        _mBestDefense = pFirstMinion.Defense;
        _mBestIntelligence = pFirstMinion.Intelligence;
        _mBestSpeed = pFirstMinion.Speed;
        
        foreach (var pMinion in _minionsAfterWave)
        {
            _mBestLife = (_mBestLife > pMinion.Value.Life)? pMinion.Value.Life: _mBestLife;
            _mBestDefense = (_mBestDefense > pMinion.Value.Defense)? pMinion.Value.Defense: _mBestDefense;
            _mBestIntelligence = (_mBestIntelligence > pMinion.Value.Intelligence)? pMinion.Value.Intelligence: _mBestIntelligence;
            _mBestSpeed = (_mBestSpeed > pMinion.Value.Speed)? pMinion.Value.Speed: _mBestSpeed;
        }
    }

    private void DoCalculateFitnessDistance()
    {
        foreach (var pMinion in _minionsAfterWave)
        {
            pMinion.Key.Fitness = pMinion.Value.TraveledDistance + pMinion.Value.Life;
        }
    }
    
    private void DoCalculateFitnessSpeed()
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
    }
    
    private void DoCalculateFitnessDefense()
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
    }
    
    private void DoCalculateFitnessIntelligence()
    {
        float pBestIntelligence = _minionsAfterWave.First().Value.Intelligence;
        foreach (var pMinion in _minionsAfterWave)
        {
            if (pMinion.Value.Intelligence > pBestIntelligence)
            {
                pBestIntelligence = pMinion.Value.Intelligence;
            }
        }

        foreach (var pMinion in _minionsAfterWave)
        {
            pMinion.Key.Fitness = pMinion.Value.Intelligence/pBestIntelligence;
        }
    }
    
    private void DoCalculateFitnessIntelligenceSpeed()
    {
        float pBestIntelligence = _minionsAfterWave.First().Value.Intelligence;
        float pBestSpeed = _minionsAfterWave.First().Value.Speed;
        foreach (var pMinion in _minionsAfterWave)
        {
            if (pMinion.Value.Intelligence > pBestIntelligence)
            {
                pBestIntelligence = pMinion.Value.Intelligence;
            }
                    
            if (pMinion.Value.Speed > pBestSpeed)
            {
                pBestSpeed = pMinion.Value.Speed;
            }
        }

        foreach (var pMinion in _minionsAfterWave)
        {
            pMinion.Key.Fitness = (pMinion.Value.Intelligence/pBestIntelligence * 3) + (pMinion.Value.Speed/pBestSpeed * 1);
        } 
    }
    
    private void DoCalculateFitnessIntelligenceDefense()
    {
        float pBestIntelligence = _minionsAfterWave.First().Value.Intelligence;
        float pBestMitigatedDamage = _minionsAfterWave.First().Value.MitigatedDamage;
        foreach (var pMinion in _minionsAfterWave)
        {
            if (pMinion.Value.Intelligence > pBestIntelligence)
            {
                pBestIntelligence = pMinion.Value.Intelligence;
            }
                    
            if (pMinion.Value.MitigatedDamage > pBestMitigatedDamage)
            {
                pBestMitigatedDamage = pMinion.Value.MitigatedDamage;
            }
        }

        foreach (var pMinion in _minionsAfterWave)
        {
            pMinion.Key.Fitness = (pMinion.Value.Intelligence/pBestIntelligence * 3) + (pMinion.Value.MitigatedDamage/pBestMitigatedDamage * 1);
        }
    }
    
    #endregion
    public void Crossover(List<MinionData> pFrom)
    {
        _minionsFromCrossover = new List<MinionData>();
        
        for(int i = 0; i < _minionsQuantity/2; i++)
        {
            MinionData parent1 = RouletteSelection(pFrom);
            MinionData parent2 = RouletteSelection(pFrom);
            while (parent1 == parent2)
            {
                parent2 = RouletteSelection(pFrom);
            }

            doCrossover(parent1, parent2);
        }
    }

    private void doCrossover(MinionData minion1, MinionData minion2)
    {       
        float[] data1 = TransformDataToVector(minion1);
        float[] data2 = TransformDataToVector(minion2);
        
        float[] child1 = new float[4];
        float[] child2 = new float[4];
        
        int cut_pos = Random.Range(1, data1.Length);

        for (int i = 0; i < cut_pos; i++)
        {
            child1[i] = data1[i];
            child2[i] = data2[i];
        }
        
        for (int i = cut_pos; i < data1.Length; i++)
        {
            child1[i] = data2[i];
            child2[i] = data1[i];
        }

        MinionData minionDataChild1 = TransformVectorToData(child1);
        MinionData minionDataChild2 = TransformVectorToData(child2);

        _minionsFromCrossover.Add(minionDataChild1);
        _minionsFromCrossover.Add(minionDataChild2);
    }

    private MinionData RouletteSelection(List<MinionData> pFrom)
    {
        float fMax = pFrom.Sum(data => data.Fitness);
        float fMinimum = Random.Range(0, fMax);
        float fCurrent = 0;
        foreach (var data in pFrom)
        {
            fCurrent += data.Fitness;
            if (fCurrent > fMinimum)
            {
                return data;
            }
        }

        int pos = Random.Range(0, pFrom.Count);
        return pFrom.ElementAt(pos);
    }

    private List<MinionData> SortMinions(Dictionary<MinionData, FitnessData> pFrom)
    {
        List<MinionData> ordernedList = pFrom.Keys.ToList();

        ordernedList.Sort((pair1, pair2) => pair1.Fitness.CompareTo(pair2.Fitness));
        return ordernedList;
    }

    private List<MinionData> Elitization(List<MinionData> pFrom)
    {
        int amoutToRemoveTop = Mathf.RoundToInt((_elitismTopTierPercent / 100) * pFrom.Count);
        int amoutToRemoveBottom = Mathf.RoundToInt((_elitismBottomTierPercent/ 100) * pFrom.Count);
        
        for (int i = 0, j = pFrom.Count - 1; i < amoutToRemoveTop; i++, j--)
        {
            pFrom.Remove(pFrom[j]);
        }
        pFrom.Reverse();
        
        for (int i = 0, j = pFrom.Count - 1; i < amoutToRemoveBottom; i++, j--)
        {
            pFrom.Remove(pFrom[j]);
        }
        pFrom.Reverse();

        return pFrom;

    }

    private void Mutation()
    {
        int qnty = Mathf.RoundToInt((_mutationPercent / 100) * _minionsFromCrossover.Count);
        for(int i = 0; i < qnty; i++) {
            MinionData tSelected = RouletteSelection(_minionsFromCrossover);
            DoMutation(tSelected);
        }
    }

    private void DoMutation(MinionData pMinionData)
    {
          MinionData tNewMinion = new MinionData(pMinionData);
          float[] data = TransformDataToVector(tNewMinion);
          int pos = Random.Range(0, data.Length - 1);

          switch (pos)
          {
              // Life
              case 0:
                  data[pos] = GenerateRandomLife();
                  break;
              // DefPoints
              case 1:
                  data[pos] = GenerateRandomDefense();
                  break;
              // Speed
              case 2:
                  data[pos] = GenerateRandomSpeed();
                  break;
              // Intelligence
              case 3:
                  data[pos] = GenerateRandomIntelligence();
                  break;
          }

          tNewMinion = TransformVectorToData(data);
          _minionsFromCrossover.Remove(pMinionData);
          _minionsFromCrossover.Add(tNewMinion);

    }

    private float[] TransformDataToVector(MinionData pData)
    {
        float[] tempData = new float[4];
        tempData[0] = pData.FullLife;
        tempData[1] = pData.DefPoints;
        tempData[2] = pData.SpeedValue;
        tempData[3] = pData.Intelligence;

        return tempData;
    }

    private MinionData TransformVectorToData(float[] pVector)
    {
        MinionData tMinionData = new MinionData();
        tMinionData.FullLife = pVector[0];
        tMinionData.DefPoints = pVector[1];
        tMinionData.SpeedValue = pVector[2];
        tMinionData.Intelligence = pVector[3];
        return tMinionData;
    }
    
    
}
