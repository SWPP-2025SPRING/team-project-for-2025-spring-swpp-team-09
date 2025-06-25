using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 4f;
    public float patrolDistance = 8f;
    public LayerMask groundLayers;

    private Vector3 startPosition;
    private Vector3 direction = Vector3.forward;

    private void Start()
    {
        startPosition = transform.position;
        direction = transform.forward;
    }

    public void Patrol()
    {
        if (!IsGroundAhead())
        {
            ReverseDirection();
            return;
        }

        transform.position += transform.forward * patrolSpeed * Time.deltaTime;

        float distFromStart = Vector3.ProjectOnPlane(transform.position - startPosition, Vector3.up).magnitude;
        if (distFromStart >= patrolDistance)
        {
            ReverseDirection();
        }
    }

    private bool IsGroundAhead()
    {
        Vector3 origin = transform.position + transform.forward * 0.5f + Vector3.up * 0.5f;
        return Physics.Raycast(origin, Vector3.down, 2f, groundLayers);
    }

    private void ReverseDirection()
    {
        transform.Rotate(0,180f,0);
        direction = transform.forward;
    }

    public float GetCurrentSpeed()
    {
        return patrolSpeed;
    }
}
