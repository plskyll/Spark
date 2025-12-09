using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float waitTimeMin = 1f;
    [SerializeField] private float waitTimeMax = 3f;
    
    [SerializeField] private float detectionRange = 8f;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private float waitTimer;
    private bool isWaiting;
    private bool isDead = false; 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("ERROR: EnemyAI could not find Player! Check if Player object has 'Player' tag.");
        }

        SetRandomDestination();
        agent.speed = patrolSpeed;
        
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("ERROR: Enemy is not on NavMesh! Ensure NavMesh is baked.");
        }
    }

    private void Update()
    {
        if (isDead) return; 
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void ChasePlayer()
    {
        isWaiting = false;
        agent.speed = chaseSpeed;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = Random.Range(waitTimeMin, waitTimeMax);
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    isWaiting = false;
                    SetRandomDestination();
                }
            }
        }
    }

    private void SetRandomDestination()
    {
        if (!agent.isOnNavMesh) return;

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void SetDeathState()
    {
        isDead = true;
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        enabled = false; 
    }
}