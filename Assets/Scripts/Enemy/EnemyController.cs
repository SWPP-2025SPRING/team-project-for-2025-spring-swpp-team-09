using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyMovementController movementController;
    public EnemyAnimationController animationController;

    public int maxHP = 100;
    private int currentHP;

    // for testing
    private bool isDead = false;
    public int CurrentHP => currentHP;
    public bool IsDead => isDead;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        movementController?.Patrol();
        
        if (animationController != null)
        {
            animationController.SetSpeed(movementController.GetCurrentSpeed());
        }
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
        Destroy(gameObject, 1.5f);
    }
}
