using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetSpeed (float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }
}
