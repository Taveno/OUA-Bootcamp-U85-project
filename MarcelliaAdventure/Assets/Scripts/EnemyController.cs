using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] Transform target;

    [SerializeField] float limitDistance;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        agent.speed = 1f;

        float distance = Vector3.Distance(transform.position, target.position);

        if (Mathf.Abs(distance) < limitDistance)
            agent.SetDestination(target.position);
        else
            agent.SetDestination(transform.position); // Stop following by setting the destination to current position
    }





}
