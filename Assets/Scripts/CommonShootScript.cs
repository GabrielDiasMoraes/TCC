using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonShootScript : MonoBehaviour
{

    private bool _isGoing;
    Vector3 _target;
    Transform _initPosition;

    [SerializeField]
    private ParticleSystem _particleSystem;
    
    public bool Setup(Transform pTarget, Transform pInitPosition)
    {
        if (_isGoing)
        {
            return false;
        }
        _target = pTarget.position;
        _initPosition = pInitPosition;
        transform.position = pInitPosition.position;
        _isGoing = true;
        //var tEmissionModule = _particleSystem.emission;
        //tEmissionModule.enabled = true;
        return true;
    }

    void Start()
    {
        var tEmissionModule = _particleSystem.emission;
        tEmissionModule.enabled = false;
    }

    //Todo: Make shoot stop in final position, and so will disappear in the ground
    void Update()
    {
        if (_isGoing)
        {
            transform.LookAt(_target);
            transform.position += transform.forward * 2 * Time.deltaTime;
            Vector3 shootPos = transform.position;
            shootPos.y = 0;
            Vector3 targetPos = _target;
            targetPos.y = 0;
            Debug.Log(Vector3.Distance(targetPos, shootPos) <= 0.1f);
            if (Vector3.Distance(targetPos, shootPos) <= 0.1f)
            {
                _isGoing = false;
                //var tEmissionModule = _particleSystem.emission;
                //tEmissionModule.enabled = false;
                //transform.position = _initPosition.position;
            }
        }
        //else
        //{
        //    transform.position = _initPosition.position;
        //}
    }
}
