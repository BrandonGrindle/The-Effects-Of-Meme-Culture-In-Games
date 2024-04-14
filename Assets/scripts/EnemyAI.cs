using StarterAssets;
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
    [SerializeField] private LayerMask WhatIsGround, WhatIsPlayer;

    [SerializeField] private Vector3 walkPoint;
    bool walkpointSet;
    [SerializeField] private float walkrange;
    [SerializeField] private float speed;

    [SerializeField] private float SightRange, AttackRange;
    [SerializeField] private bool InAttackRange, InSightRange;
    [SerializeField] private int attackDelay;
    bool alreadyAttacked;

    [SerializeField] private Items ItemDrop;

    private Animator animator;
    private int _animIDAttack;
    private int _animIDDamaged;
    private int _animIDDeath;
    private int _animIDrun;
    private void Awake()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        _animIDAttack = Animator.StringToHash("Attack");
        _animIDDamaged = Animator.StringToHash("Damaged");
        _animIDDeath = Animator.StringToHash("Dead");
        _animIDrun = Animator.StringToHash("run");
    }

    private void Update()
    {
        InSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        InAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (!InSightRange && !InAttackRange) { patrol(); }
        if (InSightRange && !InAttackRange) { chase(); }
        if (InSightRange && InAttackRange) { Attack(); }
    }

    public void DamageTaken(int damage)
    {
        health -= (damage / defense);
        animator.SetBool(_animIDDamaged, true);
        if (health <= 0)
        {
            animator.SetBool(_animIDDeath, true);
            EventManager.Instance.cstmevents.SkeletonKilled();
            InventoryManager.Instance.AddItem(ItemDrop);
            Destroy(this.gameObject);
        }
    }

    private void patrol()
    {
        if (!walkpointSet) { SearchWalkPoint(); }

        if (walkpointSet)
        {
            //Debug.Log(" walk point set");
            agent.SetDestination(walkPoint);

            Vector3 disttoWP = transform.position - walkPoint;
            if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                walkpointSet = false;
            }
            else if (disttoWP.magnitude < 1f)
            {
                walkpointSet = false;
            }
            animator.SetFloat(_animIDrun, agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat(_animIDrun, 0f);
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkrange, walkrange);
        float randomX = Random.Range(-walkrange, walkrange);
        NavMeshHit hit;

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (NavMesh.SamplePosition(randomPoint, out hit, walkrange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkpointSet = true;
            return;
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
            ThirdPersonController.instance.PlayerDamaged(Damage);
            animator.SetBool(_animIDAttack, true);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackDelay);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
