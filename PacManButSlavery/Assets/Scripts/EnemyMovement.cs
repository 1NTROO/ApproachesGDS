using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    enum EnemyState
    {
        Patrolling,
        Chasing,
        Returning
    }

    private EnemyState currentState = EnemyState.Patrolling;

    public Transform[] patrolPoints;
    private int currentPatrolIndex;
    private Transform playerTransform;
    private NavMeshAgent agent;

    [SerializeField] private int enemyID; // 0 for enemy1, 1 for enemy2, etc.
    [SerializeField] private float chaseDistance = 7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false; // Disable automatic rotation
        agent.updateUpAxis = false; // Disable automatic up axis

        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (patrolPoints.Length == 0)
        {
            GameManager.Instance.GetPatrolPointsForEnemy(enemyID, out patrolPoints);
        }

        if (GameManager.Instance.TransformPlayer != null && Vector3.Distance(transform.position, GameManager.Instance.TransformPlayer.position) < chaseDistance)
        {
            playerTransform = GameManager.Instance.TransformPlayer;
            currentState = EnemyState.Chasing;
        }
        else if (currentState == EnemyState.Chasing)
        {
            currentState = EnemyState.Returning;
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
    }

    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = points;
        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

    }

    void Chase()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
        agent.speed = 3.5f; // Increase speed when chasing
    }

    void ReturnToPatrol()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            currentState = EnemyState.Patrolling;
        }
        agent.speed = 1f; // Reset speed to normal when returning
    }
}
