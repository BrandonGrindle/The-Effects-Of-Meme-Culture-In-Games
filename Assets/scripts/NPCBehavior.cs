using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    public Items item;
    public Collider MainCol;
    public Animator animator;
    public bool captured = false;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Vector3 walkPoint;
    bool walkpointSet;
    [SerializeField] private float walkrange;
    [SerializeField] private LayerMask WhatIsGround;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        patrol();
    }

    public void pickupItem()
    {
        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }

    public void Captured()
    {
        captured = true;
        animator.enabled = false;       
        
    }

    private void patrol()
    {
        if (!walkpointSet) { SearchWalkPoint(); }

        if (walkpointSet)
        {
            //Debug.Log(" walk point set");
            agent.SetDestination(walkPoint);
        }

        Vector3 disttoWP = transform.position - walkPoint;

        if (disttoWP.magnitude < 1f)
        {
            walkpointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkrange, walkrange);
        float randomX = Random.Range(-walkrange, walkrange);
        NavMeshHit hit; // Used to store the information returned from SamplePosition

        // Try multiple times to find a valid walk point
        int attempts = 10;
        while (attempts > 0)
        {
            randomZ = Random.Range(-walkrange, walkrange);
            randomX = Random.Range(-walkrange, walkrange);

            Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            // Check if the randomPoint is within the navmesh and find the closest point on the NavMesh to randomPoint
            if (NavMesh.SamplePosition(randomPoint, out hit, walkrange, NavMesh.AllAreas))
            {
                walkPoint = hit.position; // Use the position hit by SamplePosition which is on the NavMesh
                walkpointSet = true;
                return; // Exit the function if a valid point is found
            }

            attempts--; // Decrement the number of attempts left
        }
    }
}
