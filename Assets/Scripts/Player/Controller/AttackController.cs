using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private CombatEventChannel combatEventChannel;
    [SerializeField] private SoundEventChannel soundEventChannel;
    [SerializeField] private AnimationController animationController;
    
    [SerializeField] private Transform meleePoint;
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private int meleeDamage = 50;
    private bool isAttackingThisFrame = false;

    public void HandleAttackInput(PlayerInputReader input)
    {
        isAttackingThisFrame = false;

        if (input.MeleePressed)
        {
            PerformMeleeAttack();
            input.ConsumeMelee();
            isAttackingThisFrame = true;
        }
    }
    
    public bool IsAttackingThisFrame()
    {
        return isAttackingThisFrame;
    }

    private void PerformMeleeAttack()
    {
        soundEventChannel?.RaisePlaySFX("punch");
        animationController?.TriggerAttack();

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
