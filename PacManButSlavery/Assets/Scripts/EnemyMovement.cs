using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex;
    private NavMeshAgent agent;

    [SerializeField] private int enemyID; // 0 for enemy1, 1 for enemy2, etc.

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

        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
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
}
