using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public float patrolSpeed = 3f;
    private Vector3 direction = Vector3.forward;

    public void Patrol()
    {
        transform.Translate(direction * patrolSpeed * Time.deltaTime);

        if (transform.position.z > 10f || transform.position.z < -10f)
        {
            direction = -direction;
        }
    }
}
