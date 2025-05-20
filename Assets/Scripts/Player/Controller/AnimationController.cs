using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioClip[] footstepClips;
    [Range(0f, 1f)] [SerializeField] private float volume = 0.5f;

    private CharacterController characterController;

    private int animIDSpeed;
    private int animIDMotionSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        animIDSpeed = Animator.StringToHash("Speed");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
    }

    public void UpdateMovement(float animationBlend, float inputMagnitude)
    {
        animator.SetFloat(animIDSpeed, animationBlend);
        animator.SetFloat(animIDMotionSpeed, inputMagnitude);
    }

    public void SetGrounded(bool grounded)
    {
        animator.SetBool(animIDGrounded, grounded);
    }

    public void TriggerJump()
    {
        animator.SetTrigger(animIDJump);
    }

    public void SetFreeFall(bool state)
    {
        animator.SetBool(animIDFreeFall, state);
    }

    public void OnFootstep(AnimationEvent animationEvent)
    {
        if (footstepClips.Length == 0 || animationEvent.animatorClipInfo.weight < 0.5f) return;

        var index = Random.Range(0, footstepClips.Length);
        Vector3 pos = transform.position + characterController.center;
        AudioSource.PlayClipAtPoint(footstepClips[index], pos, volume);
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < 0.5f) return;

        Vector3 pos = transform.position + characterController.center;
        AudioSource.PlayClipAtPoint(landingClip, pos, volume);
    }
}
