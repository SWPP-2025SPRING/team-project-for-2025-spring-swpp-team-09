using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private float deathDelay = 1.5f;
    [SerializeField] private SoundEventChannel soundEventChannel;
    [SerializeField] private EnemyAnimationController animationController;

    private int currentHP;
    private bool isDead = false;

    public int CurrentHP => currentHP;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        soundEventChannel?.RaisePlaySFX("enemy_hit");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animationController?.PlayDeath();
        soundEventChannel?.RaisePlaySFX("enemy_death");
        Destroy(gameObject, deathDelay);
    }
}
