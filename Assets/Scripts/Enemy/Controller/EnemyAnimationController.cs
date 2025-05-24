using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlayWalk()
    {
        Debug.Log("PlayWalk() called");
        animator.Play("Walk");
    }

    public void PlayDeath()
    {
        animator.Play("Death");
    }
}
