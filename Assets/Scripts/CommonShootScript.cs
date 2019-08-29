using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonShootScript : MonoBehaviour
{

    private bool _isGoing;
    Transform _target;
    Transform _initPosition;

    [SerializeField]
    private ParticleSystem _particleSystem;
    
    public bool Setup(Transform pTarget, Transform pInitPosition)
    {
        if (_isGoing)
        {
            return false;
        }
        _target = pTarget;
        _initPosition = pInitPosition;
        transform.position = pInitPosition.position;
        _isGoing = true;
        var tEmissionModule = _particleSystem.emission;
        tEmissionModule.enabled = true;
        return true;
    }

    void Start()
    {
        var tEmissionModule = _particleSystem.emission;
        tEmissionModule.enabled = false;
    }

    void Update()
    {
        if (_isGoing)
        {
            transform.LookAt(_target);
            transform.position += transform.forward * 20 * Time.deltaTime;
            if (Vector3.Distance(_target.transform.position, transform.position) <= 1)
            {
                _isGoing = false;
                var tEmissionModule = _particleSystem.emission;
                tEmissionModule.enabled = false;
                transform.position = _initPosition.position;
            }
        }
        else
        {
            transform.position = _initPosition.position;
        }
    }
}
