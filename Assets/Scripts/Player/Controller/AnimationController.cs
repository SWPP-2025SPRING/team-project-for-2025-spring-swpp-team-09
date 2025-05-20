using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int animIDSpeed;
    private int animIDMotionSpeed;

    void Awake()
    {
        AssignIDs();
    }

    private void AssignIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    public void UpdateAnimation(float speed)
    {
        animator.SetFloat(animIDSpeed, speed);
        animator.SetFloat(animIDMotionSpeed, 1f);
    }

    public void TriggerJump() => animator.SetTrigger("Jump");
}
