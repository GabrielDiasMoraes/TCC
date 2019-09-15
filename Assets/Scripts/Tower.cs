using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float _radiusRange;

    [SerializeField] private float _damage;

    [SerializeField] private float _shootDelay;

    [SerializeField] private GameObject _shootGameObject;

    [SerializeField] private GameObject _cannonRotationCenter;

    [SerializeField] private Transform _cannonBallExit;
    
    private GameObject[] _minions;

    private GameObject _nearestMinion;

    private float _nearestMinionDistance;

    private float deltaTIme;

    //Todo: Make a array for the shoots (some like 3/cdTime)
    private CommonShootScript shootScript;
    

    private void Start()
    {
        deltaTIme = _shootDelay;
        shootScript = _shootGameObject.GetComponent<CommonShootScript>();
    }


    void Update()
    {
        deltaTIme += Time.deltaTime;
        _minions = PopulationController.Instance._aliveMinions.ToArray();
        if(_minions.Length == 0)
            return;
        _nearestMinion = _minions[0];
        _nearestMinionDistance = Vector3.Distance(_minions[0].transform.position, transform.position);
        for(int i = 0; i < _minions.Length; i++)
        {
            float tDistance = Vector3.Distance(_minions[i].transform.position, transform.position);
            if (_nearestMinionDistance > tDistance)
            {
                _nearestMinion = _minions[i];
                _nearestMinionDistance = tDistance;
            }
        }

        if (_nearestMinionDistance <= _radiusRange )
        {
            Vector3 whereToLook = _nearestMinion.transform.position;
            whereToLook.y = _cannonRotationCenter.transform.position.y;
            _cannonRotationCenter.transform.LookAt(whereToLook);
            
            if (deltaTIme > _shootDelay)
            {
                Minion tempMinion = _nearestMinion.GetComponent<Minion>();
                if (tempMinion != null)
                {
                    //_shootGameObject.transform.position = _cannonBallExit.position;
                    if (shootScript.Setup(tempMinion.transform, _cannonBallExit))
                    {
                        tempMinion.TakeDamage(_damage);
                        deltaTIme = 0;
                    }
                }
            }
        }

    }
    
    
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radiusRange);
    }
}
