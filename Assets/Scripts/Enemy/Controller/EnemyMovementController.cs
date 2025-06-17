using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 15f;
    public float patrolDistance = 4f;
    public LayerMask groundLayers;

    private Vector3 startPosition;
    private Vector3 direction = Vector3.forward;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void Patrol()
    {
        if (!IsGroundAhead())
        {
            direction = -direction;
            return;
        }

        transform.Translate(direction.normalized * patrolSpeed * Time.deltaTime, Space.World);

        float distFromStart = Vector3.ProjectOnPlane(transform.position - startPosition, Vector3.up).magnitude;
        if (distFromStart >= patrolDistance)
        {
            direction = -direction;
        }
    }

    private bool IsGroundAhead()
    {
        Vector3 origin = transform.position + direction.normalized * 0.5f + Vector3.up * 0.5f;
        return Physics.Raycast(origin, Vector3.down, 2f, groundLayers);
    }
}
