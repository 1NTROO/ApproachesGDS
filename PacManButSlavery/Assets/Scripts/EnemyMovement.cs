using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Returning
    }

    private EnemyState currentState = EnemyState.Patrolling;
    public EnemyState CurrentState { get { return currentState; } }

    public List<Transform> patrolPoints;
    private int currentPatrolIndex;
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator animator;
    private float animationSpeed = 1f;

    [SerializeField] private int enemyID; // 0 for enemy1, 1 for enemy2, etc.
    [SerializeField] private float chaseDistance = 7f;

    [Header("Audio")]
    [SerializeField] private AudioClip chaseSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false; // Disable automatic rotation
        agent.updateUpAxis = false; // Disable automatic up axis

        animator = GetComponentInChildren<Animator>();


        if (patrolPoints.Count == 0)
        {
            GameManager.Instance.GetPatrolPointsForEnemy(enemyID, out patrolPoints);
        }

        if (patrolPoints.Count > 0)
        {
            currentPatrolIndex = 0;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.PlayerHasContraband)
        {
            if (PlayerInSightCheck())
            {
                currentState = EnemyState.Chasing;
                playerTransform = FindAnyObjectByType<PlayerMovement>().transform;
            }
            else
            {
                currentState = EnemyState.Patrolling;
            }
        }
        else
        {
            currentState = EnemyState.Patrolling;
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Returning:
                ReturnToPatrol();
                break;
        }


        var pos = transform.position;
        pos.z = 0; // Keep the enemy on the same Z plane
        transform.position = pos;

        Vector3 normalizedVelocity = Vector3.ClampMagnitude(agent.velocity, 1f);

        animator.SetFloat("velocityX", normalizedVelocity.x);
        animator.SetFloat("velocityY", normalizedVelocity.y);

        animator.SetFloat("speedMult", animationSpeed);
    }

    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = new List<Transform>(points);
        if (patrolPoints.Count > 0)
        {
            currentPatrolIndex = 0;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    bool PlayerInSightCheck()
    {
        Vector3 start = transform.position;
        Vector3 dir = (FindAnyObjectByType<PlayerMovement>().transform.position - transform.position).normalized;
        float distance = chaseDistance * PlayerStatsManager.Instance.ThreatDetectionModifier;

        // Debug.DrawRay(start, dir * distance, Color.black);

        RaycastHit2D sightTest = Physics2D.Raycast(start, dir, distance, ~LayerMask.GetMask("Enemy"));
        if (sightTest.collider != null)
        {
            Debug.Log(sightTest.collider.gameObject.name);
            if (sightTest.collider.gameObject.name == "Player")
            {
                Debug.Log("Found the player");
                return true;
            }
        }
        return false;

    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            Debug.Log("Patrolling to point " + currentPatrolIndex + ": " + patrolPoints[currentPatrolIndex].position);
        }

    }

    void Chase()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
        agent.speed = 3.5f; // Increase speed when chasing
        animationSpeed = 2f; // Increase animation speed when chasing
        
        AudioManager.Instance.PlaySound(chaseSound, 0.05f);
    }

    void ReturnToPatrol()
    {
        if (patrolPoints.Count > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            currentState = EnemyState.Patrolling;
        }
        agent.speed = 1f; // Reset speed to normal when returning
        animationSpeed = 1f; // Reset animation speed to normal when returning
    }
}
