using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
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
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StarterAssets.ThirdPersonController player = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (player != null)
            {
                player.ApplySpeedModifier(0.8f, 2f);
            }
        }
    }

}