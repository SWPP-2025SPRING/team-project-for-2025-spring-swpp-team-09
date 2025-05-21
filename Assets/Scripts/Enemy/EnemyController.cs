using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyMovementController movementController;
    public EnemyAnimationController animationController;

    public int maxHP = 100;
    private int currentHP;

    [SerializeField] private CollisionEventChannel collisionChannel;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        movementController?.Patrol();
        //animationController?.PlayIdle();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"Enemy damaged: {damage}, HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animationController?.PlayDeath();
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collisionChannel?.RaisePlayerHit(other, 0.8f, 2f);
        }
    }
}
