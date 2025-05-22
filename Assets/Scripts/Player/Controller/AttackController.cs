using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private CombatEventChannel combatEventChannel;
    [SerializeField] private Transform meleePoint;
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private int meleeDamage = 50;

    public void HandleAttackInput(PlayerInputReader input)
    {
        if (input.MeleePressed)
        {
            PerformMeleeAttack();
            input.ConsumeMelee();
        }
    }

    private void PerformMeleeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(meleePoint.position, meleeRange);
        Debug.Log("Attack attempted. Detected: " + hitEnemies.Length);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Enemy found: " + enemy.name);
                combatEventChannel.RaiseAttack(enemy, meleeDamage);
            }
        }
    }
}
