using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public int health = 3;
    public int Damage = 5;
    public int defense = 5;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask WhatIsGround,WhatIsPlayer;

    [SerializeField] private Vector3 walkPoint;
    bool walkpointSet;
    [SerializeField] private float walkrange;
    [SerializeField] private float speed;

    [SerializeField] private float SightRange, AttackRange;
    [SerializeField] private bool InAttackRange, InSightRange;
    [SerializeField] private int attackDelay;
    bool alreadyAttacked;

    private void Awake()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        InSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        InAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if(!InSightRange && !InAttackRange) { patrol(); }
        if (InSightRange && !InAttackRange) { chase(); }
        if (InSightRange && InAttackRange) { Attack(); }
    }

    public void DamageTaken(int damage)
    {
        health -= (damage / defense);

        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void patrol()
    {
        if(!walkpointSet) { SearchWalkPoint();  }

        if(walkpointSet) {
            Debug.Log(" walk point set");
            agent.SetDestination(walkPoint);
        }

        Vector3 disttoWP = transform.position - walkPoint; 

        if(disttoWP.magnitude < 1f)
        {
            walkpointSet = false;
        } 
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkrange, walkrange);
        float randomX = Random.Range(-walkrange, walkrange);

        walkPoint = new Vector3(transform.position.x + randomX,transform.position.y,transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
        {
            walkpointSet = true;
        }
    }

    private void chase()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackDelay);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
