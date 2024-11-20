using UnityEngine;
using UnityEngine.AI;

public class NavmeshAgentObject : MonoBehaviour
{
    [SerializeField] private float randomMoveRadius = 10f;
    private NavMeshAgent agent;

    private Vector3 destination;

    public bool IsDestination => Vector3.Distance(transform.position, destination + (transform.position.y - destination.y) * Vector3.up) < 0.1f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError(name + " not found component nav mesh agent!");
    }

    public void StopMoving()
    {
        SetDestination(transform.position);
    }

    public void SetDestination(Vector3 target)
    {
        if (!agent.enabled)
            return;
        if (!agent.isActiveAndEnabled)
            return;

        destination = target;
        agent.SetDestination(target);

        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
    }

    public void RandomMove()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * randomMoveRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, randomMoveRadius, 1))
        {
            Vector3 finalPosition = hit.position;
            SetDestination(finalPosition);
        }
        else
        {
            RandomMove();
        }
    }
}
