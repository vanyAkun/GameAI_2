using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

    public Transform bulletPosition;
    public GameObject bulletPrefab;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GeneratePatrolPoints();
        navMeshAgent.SetDestination(PatrolPoints[nextPatrolPoint]);
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void GeneratePatrolPoints()
    {
        PatrolPoints = new Vector3[5];
        for (int i = 0; i < PatrolPoints.Length; i++)
        {
            PatrolPoints[i] = new Vector3(
                UnityEngine.Random.Range(-10f, 10f), // Random X within range
                0f,                                  // Assuming Y is ground level
                UnityEngine.Random.Range(-10f, 10f)  // Random Z within range
            );
        }
    }
    void Update()
    {
        SwitchState();
    }
    void Fire()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + FireRate;

            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<Bullet>()?.InitializeBullet(transform.rotation * Vector3.forward);

        }
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
        navMeshAgent.ResetPath(); // Reset the NPC's path
        navMeshAgent.isStopped = true; // Explicitly stop the NPC's movement
        meshRenderer.material = AttackMaterial; // Change material to attack material
        transform.LookAt(Player); // Ensure the NPC looks at the player

        Fire(); // Attempt to fire a bullet

        // Check if the player is out of attack range
        if (Vector3.Distance(transform.position, Player.position) > AttackRange)
        {
            navMeshAgent.isStopped = false; // Allow the NPC to move again
            currentState = NPCStates.Chase;
        }
    }

    private void Chase()
    {
        meshRenderer.material = ChaseMaterial; // Change material to chase material
        navMeshAgent.SetDestination(Player.position); // Set destination to player's position

        // Check if player is within attack range
        if (Vector3.Distance(transform.position, Player.position) <= AttackRange)
        {
            currentState = NPCStates.Attack;
        }
        // Check if player is out of chase range
        else if (Vector3.Distance(transform.position, Player.position) > ChaseRange)
        {
            navMeshAgent.ResetPath(); // Reset the path
            currentState = NPCStates.Patrol;
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, Player.position) < ChaseRange)
        {
            currentState = NPCStates.Chase;
            return; // Exit the method to avoid executing remaining code when state changes
        }

        // If the NPC is not moving
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            meshRenderer.material = PatrolMaterial; // Change material to patrol material

            // Increment patrol point and reset if needed
            nextPatrolPoint = (nextPatrolPoint + 1) % PatrolPoints.Length;
            navMeshAgent.SetDestination(PatrolPoints[nextPatrolPoint]);
        }
    }
}

