using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float _radiusRange;

    [SerializeField] private float _damage;

    [SerializeField] private float _shootDelay;

    [SerializeField] private GameObject _cannonRotationCenter;

    [SerializeField] private ParticleSystem _explosion;

    [SerializeField] private ParticleSystem _minionExplosion;

    [SerializeField]  private Transform _exit;
    
    private GameObject[] _minions;

    private Minion _target;

    private bool _bHasTarget;
   
    private float _nearestMinionDistance;

    private float deltaTIme;

    private void Start()
    {
        deltaTIme = _shootDelay;
        _bHasTarget = false;
    }


    void Update()
    {
        if (_bHasTarget)
        {
            float distance = Vector3.Distance(_target.transform.position, transform.position);
            if (distance <= _radiusRange)
            {
                Vector3 whereToLook = _target.transform.position;
                whereToLook.y = _cannonRotationCenter.transform.position.y;
                _cannonRotationCenter.transform.LookAt(whereToLook);
                deltaTIme += Time.deltaTime;
                if (deltaTIme > _shootDelay)
                {
                    _explosion.transform.position = _exit.position;
                    _explosion.Play();
                    _minionExplosion.transform.position = _target.transform.position;
                    _minionExplosion.Play();
                    _target.TakeDamage(_damage);
                    deltaTIme = 0;

                    if (!_target.isAlive)
                    {
                        _bHasTarget = false;
                    }
                }
            }
            else
            {
                _bHasTarget = false;
            }
        }

        if (_bHasTarget) return;
        _minions = PopulationController.Instance._aliveMinions.ToArray();
        if(_minions.Length == 0)
            return;
        
        _nearestMinionDistance = Vector3.Distance(_minions[0].transform.position, transform.position);
        
        foreach (var minion in _minions)
        {
            Minion tMinion = minion.GetComponent<Minion>();
            float tDistance = Vector3.Distance(minion.transform.position, transform.position);
            if (_nearestMinionDistance >= tDistance && tMinion.isAlive && tDistance <= _radiusRange)
            {
                _nearestMinionDistance = tDistance;
                _target = tMinion;
                _bHasTarget = true;
            }
        }
    }
    
    void Shoot(Minion tMinion)
    {
        tMinion.TakeDamage(_damage);
    }
    
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radiusRange);
    }
}
