using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public float patrolSpeed = 3f;
    private Vector3 direction = Vector3.forward;

    public void Patrol()
    {
        if (gameObject.name != "TestEnemy") return;
        Debug.Log($"[{gameObject.name}] pos: {transform.position}");
        transform.Translate(direction * patrolSpeed * Time.deltaTime, Space.World);

        if (transform.position.z > 10f || transform.position.z < -10f)
        {
            direction = -direction;
        }
    }
}
