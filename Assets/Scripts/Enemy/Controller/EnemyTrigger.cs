using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private CollisionEventChannel collisionChannel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collisionChannel?.RaisePlayerHit(other, 0.5f, 1.5f);
        }
    }
}
