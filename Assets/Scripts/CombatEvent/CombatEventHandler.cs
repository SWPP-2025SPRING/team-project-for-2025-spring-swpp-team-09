using UnityEngine;

public class CombatEventHandler : MonoBehaviour
{
    [SerializeField] private CombatEventChannel channel;

    private void OnEnable() => channel.OnAttack += HandleAttack;
    private void OnDisable() => channel.OnAttack -= HandleAttack;

    private void HandleAttack(Collider col, int damage)
    {
        if (col.TryGetComponent(out EnemyController enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
