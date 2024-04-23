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
    public AudioClip[] genericResponce;

    [SerializeField] private int SpeechIntervalMin, SpeechIntervalMax;

    bool playingAudio = false;

    IEnumerator randomVoiceRange()
    {
        yield return new WaitForSeconds(Random.Range(SpeechIntervalMin, SpeechIntervalMax));
        playRandAudio();
        playingAudio = false;
    }
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
        if (!playingAudio)
        {
            playingAudio = true;
            StartCoroutine(randomVoiceRange());
        }
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
            source.volume = 1.0f;
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
        NavMeshHit hit; 
        int attempts = 10;
        while (attempts > 0)
        {
            randomZ = Random.Range(-walkrange, walkrange);
            randomX = Random.Range(-walkrange, walkrange);

            Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            if (NavMesh.SamplePosition(randomPoint, out hit, walkrange, NavMesh.AllAreas))
            {
                walkPoint = hit.position; 
                walkpointSet = true;
                return; 
            }

            attempts--; 
        }
    }

    private void SetRagdollState(bool state)
    {
        foreach (var rb in RagdollArtefact)
        {
            rb.isKinematic = !state;
        }

        if (animator != null) animator.enabled = !state;
        if (agent != null) agent.enabled = !state;

        if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = !state;
    }

    private void playRandAudio()
    {
        if (genericResponce.Length > 0)
        {
            int index = Random.Range(0, genericResponce.Length);
            source.clip = genericResponce[index];
            source.volume = .4f;
            source.Play();
        }
    }
}