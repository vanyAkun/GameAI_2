using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public enum NPCStates
    {
        Patrol,
        Chase,
        Attack
    }

    [SerializeField]
    Vector3[] PatrolPoints;
    [SerializeField]
    Transform Player;
    [SerializeField]
    Bullet Bullet;
    [SerializeField]
    Material PatrolMaterial;
    [SerializeField]
    Material ChaseMaterial;
    [SerializeField]
    Material AttackMaterial;
    [SerializeField]
    float ChaseRange = 7f;
    [SerializeField]
    float AttackRange = 4f;

    float FireRate = 2f;
    int nextPatrolPoint = 0;
    NPCStates currentState = NPCStates.Patrol;
    NavMeshAgent navMeshAgent;
    MeshRenderer meshRenderer;
    float nextShootTime = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(PatrolPoints[nextPatrolPoint]);
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        SwitchState();
    }

    private void SwitchState()
    {
        switch (currentState)
        {
            case NPCStates.Patrol:
                Patrol();
                break;
            case NPCStates.Chase:
                Chase();
                break;
            case NPCStates.Attack:
                Attack();
                break;
            default:
                Patrol();
                break;
        }
    }

    private void Attack()
    {
        // Your code
    }

    private void Chase()
    {
        // Your code
    }

    private void Patrol()
    {
        // Your code
    }
}
