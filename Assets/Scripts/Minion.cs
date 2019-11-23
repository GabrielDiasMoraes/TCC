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

    private bool _started;

    private bool _isFromCrossover;

    [SerializeField]
    private Animator _animator;

    private Lifebar _lifeBar;

    [SerializeField]
    private Renderer _renderer;

    [SerializeField] private ParticleSystem _shield;

    private bool _reachEnd;

    private float _timepassed;

    private Vector3 _destination;

    private bool isGoingWrongDest;

    private float wrongDestTime, wrongDestCooldown;

    private bool _isInvencible;

    [SerializeField] private GameObject aliveModel;

    [SerializeField] private GameObject deadModel;

    [SerializeField] private List<Renderer> _deadFragmentRenderers;
    
    #endregion

    #region Properties

    public GameObject DeadModel => deadModel;

    public GameObject AliveModel => aliveModel;

    public bool Started
    {
        get => _started;
        set => _started = value;
    }
    
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
        set
        {
            _data = value;
            SetReferencesInAbilities();
        }
    }

    public bool isAlive => _data.LifePoints > 0;

    public Lifebar LifeBar
    {
        get => _lifeBar;
        set => _lifeBar = value;
    }

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
            _renderer.material = ColorPool.Instance.GetMaterial(value, false);
            foreach (var fragmentRender in _deadFragmentRenderers)
            {
                fragmentRender.material = ColorPool.Instance.GetMaterial(value, true);
            }
        }
    }

    public float LifePoints
    {
        get => _data.LifePoints;
        set => _data.LifePoints = value;
    }

    public float DefPoints
    {
        get => _data.DefPoints;
        set => _data.DefPoints = value;
    }
    
    public float Intelligence
    {
        get => _data.Intelligence;
        set => _data.Intelligence = value;
    }

    public float SpeedValue
    {
        get => _data.SpeedValue;
        set => _data.SpeedValue = value;
    }

    public List<IAbility> Abilities
    {
        get => _data.Abilities;
        set => _data.Abilities = value;
    }

    public bool IsFromCrossover
    {
        get => _isFromCrossover;
        set => _isFromCrossover = value;
    }

    public Vector3 Destination
    {
        get => _destination;
        set => _destination = value;
    }

    public bool IsInvencible
    {
        get => _isInvencible;
        set => _isInvencible = value;
    }

    #endregion

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        wrongDestCooldown = 0;
        wrongDestTime = 0;
        isGoingWrongDest = false;
        _lifeBar.gameObject.SetActive(true);
        _isInvencible = false;
    }

    private void Update()
    {
        ExecuteAbilities();
    }

    private void LateUpdate()
    {
        if (!isGoingWrongDest)
        {
            //_animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
            float dist = Vector3.Distance(transform.position, navMeshAgent.destination);

            if (dist <= 1f && !_reachEnd)
            {
                OnFinish();
            }

            if (_reachEnd || !isAlive)
            {
                _timepassed += Time.deltaTime;
                if (_timepassed >= 3f)
                {
                    deadModel.SetActive(false);
                    aliveModel.SetActive(false);
                }
            }
        }
        else
        {
            wrongDestTime += Time.deltaTime;
            if (wrongDestTime > 2f)
            {
                navMeshAgent.destination = Destination;
                isGoingWrongDest = false;
                wrongDestCooldown = 0f;
            }
        }
        
        
        
        if (wrongDestCooldown >= 2f && !isGoingWrongDest && !DataController.Instance.HasIntelligenceBonus.Value)
        {
            bool goWrongDest = true;
            if (_data.Intelligence < 10)
            {
                int val = Random.Range(0, 10);
                goWrongDest = val >= _data.Intelligence;
            }

            if (goWrongDest)
            {
                wrongDestTime = 0f;
                isGoingWrongDest = true;
                if(navMeshAgent.gameObject.activeSelf)
                    navMeshAgent.destination = GenerateWrongDestination();
            }
        }

        wrongDestCooldown += Time.deltaTime;
        if(!_animator.isActiveAndEnabled) return;
        _animator.Play(navMeshAgent.velocity.magnitude > 0 ? "Idle" : "Stopped");
    }

    public void ResetMinion()
    {
        LifeBar.gameObject.SetActive(false);
        DeadModel.SetActive(false);
        AliveModel.SetActive(true);
        DeadModel.transform.parent = transform;
        DeadModel.transform.position = transform.position;
        AliveModel.transform.position = transform.position;
        DeadModel.transform.rotation = transform.rotation;
        AliveModel.transform.rotation = transform.rotation;
        gameObject.SetActive(false);
        _reachEnd = false;
        wrongDestTime = 0f;
        wrongDestCooldown = 0f;
        isGoingWrongDest = false;
        _timepassed = 0f;
        _isInvencible = false;
    }
    
    public Vector3 GenerateWrongDestination()
    {
        var tempTransform = transform;
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
            {
                
                return tempTransform.position + tempTransform.right * 3;
            }
            
            case 2:
            {
                return tempTransform.position - tempTransform.right * 3;
            }
                
            case 3:
            {
                return tempTransform.position - tempTransform.forward * 5;
            }
        }

        return tempTransform.position + transform.forward * 5;
    }

    public void TakeDamage(float damage)
    {
        int difficulty = DataController.Instance.CurrentDifficulty; 
        if(difficulty == 0) {}
        else
        {
            damage = (difficulty == -1)? damage - damage*0.1f: damage + damage*0.2f;
        }
        float tEndDamage = (_isInvencible)? 0 :(damage * (float)_data.DefPoints / 100);
        _data.LifePoints -= tEndDamage;
        _lifeBar.Slider.value = Mathf.Max(0, _data.LifePoints);
        _data.MitigatedDamage += damage - tEndDamage;
        
        _animator.Play("Hit");
        
        if (_data.LifePoints <= 0)
        {
            OnDie();
        }

    }

    
    private void OnDie()
    {
        _fitnessData = GenerateFitness();
        navMeshAgent.speed = 0;
        gameObject.tag = "DeadMinion";
        PopulationController.Instance.SaveMinion(_data, _fitnessData, true);
        _lifeBar.gameObject.SetActive(false);
        aliveModel.SetActive(false);
        deadModel.transform.parent = null;
        deadModel.SetActive(true);
    }

    private void OnFinish()
    {
        _fitnessData = GenerateFitness();
        navMeshAgent.speed = 0;

        gameObject.tag = "FinishMinion";
        PopulationController.Instance.SaveMinion(_data, _fitnessData);
        _reachEnd = true;
        _lifeBar.gameObject.SetActive(false);
        _animator.Play("Stopped");
    }

    private FitnessData GenerateFitness()
    {
        FitnessData fitnessData = new FitnessData();
        fitnessData.TraveledDistance = _entireDistance - navMeshAgent.remainingDistance;
        fitnessData.Life = _data.LifePoints;
        fitnessData.Speed = _data.SpeedValue;
        fitnessData.OriginalLife = _data.FullLife;
        fitnessData.MitigatedDamage = _data.MitigatedDamage;
        fitnessData.Defense = _data.DefPoints;
        fitnessData.Intelligence = _data.Intelligence; 
        return fitnessData;
    }
    
    /// <summary>AddAbility is used to give the minion (Ooze) a new Ability.</summary>
    public void AddAbility(IAbility newAbility)
    {
        if (newAbility == null) return;
        if (Abilities.Contains(newAbility)) return;
        newAbility.AddMinion(this, _shield);
        _data.AddAbility(newAbility);
    }

    public void SetReferencesInAbilities()
    {
        foreach (var ability in _data.Abilities)
        {
            ability.AddMinion(this, _shield);
        }
    }

    private void ExecuteAbilities()
    {
        foreach (var ability in Abilities)
        {
            ability.Execute();
        }
    }
    
}