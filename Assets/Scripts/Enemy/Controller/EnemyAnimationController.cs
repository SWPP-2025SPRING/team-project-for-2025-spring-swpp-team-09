using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayIdle()
    {
        animator.Play("Idle");
    }

    public void PlayDeath()
    {
        animator.Play("Death");
    }
}
