using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class NPCBehavior : MonoBehaviour
{
    public Items item;
    public Collider MainCol;
    public Animator animator;
    public int SpeedID;
    public bool captured = false;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Vector3 walkPoint;
    bool walkpointSet;
    [SerializeField] private float walkrange;
    [SerializeField] private LayerMask WhatIsGround;

    private List<Rigidbody> RagdollArtefact = new List<Rigidbody>();

    public AudioSource source;
    public AudioClip[] caughtsfx;
    private void Awake()
    {

        RagdollArtefact.AddRange(GetComponentsInChildren<Rigidbody>());

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        SpeedID = Animator.StringToHash("MoveSpeed");

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
        SetRagdollState(true);
        if (caughtsfx.Length > 0)
        {
            var index = Random.Range(0, caughtsfx.Length);
            source.clip = caughtsfx[index];
            source.volume = 1.0f; // Adjust this value as needed
            source.Play();
        }
        //animator.enabled = false;

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

            animator.SetFloat(SpeedID, agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat(SpeedID, 0f);
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

    private void SetRagdollState(bool state)
    {
        foreach (var rb in RagdollArtefact)
        {
            rb.isKinematic = !state; // If ragdoll is on, isKinematic is off, and vice versa
        }

        // Toggle the animator and agent according to the state
        if (animator != null) animator.enabled = !state;
        if (agent != null) agent.enabled = !state;

        // If the NPC has a parent Rigidbody, you may want to toggle isKinematic on that, too
        if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = !state;
    }
}