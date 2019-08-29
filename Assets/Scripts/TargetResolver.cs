using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetResolver : MonoBehaviour
{

    [SerializeField] private Transform endPoint;

    [SerializeField] private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(endPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
