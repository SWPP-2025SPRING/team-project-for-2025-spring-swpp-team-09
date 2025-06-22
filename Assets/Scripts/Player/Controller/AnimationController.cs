using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SoundEventChannel soundEventChannel;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioClip[] footstepClips;
    [Range(0f, 1f)] [SerializeField] private float volume = 0.5f;

    private CharacterController characterController;

    private int animIdSpeed;
    private int animIdMotionSpeed;
    private int animIdGrounded;
    private int animIdJump;
    private int animIdFreeFall;
    private int animIdClimb;
    private int animIdAttack;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        animIdSpeed = Animator.StringToHash("Speed");
        animIdMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIdGrounded = Animator.StringToHash("Grounded");
        animIdJump = Animator.StringToHash("Jump");
        animIdFreeFall = Animator.StringToHash("FreeFall");
        animIdClimb = Animator.StringToHash("Climb");
        animIdAttack = Animator.StringToHash("Attack");
    }

    public void Initialize()
    {
        animator.SetFloat(animIdSpeed, 0f);
        animator.SetFloat(animIdMotionSpeed, 0f);
        animator.SetBool(animIdGrounded, true);
        animator.SetBool(animIdJump, false);
        animator.SetBool(animIdFreeFall, false);
        animator.SetBool(animIdClimb, false);
    }

    public void UpdateMovement(float animationBlend, float inputMagnitude)
    {
        animator.SetFloat(animIdSpeed, animationBlend);
        animator.SetFloat(animIdMotionSpeed, inputMagnitude);
    }

    public void SetGrounded(bool grounded)
    {
        animator.SetBool(animIdGrounded, grounded);
    }

    public void TriggerJump()
    {
        animator.SetBool(animIdJump, true);
        StartCoroutine(ResetJumpFlag());
    }

    private IEnumerator ResetJumpFlag()
    {
        yield return null;
        animator.SetBool(animIdJump, false);
    }

    public void SetFreeFall(bool state)
    {
        animator.SetBool(animIdFreeFall, state);
    }

    public void SetClimb(bool state)
    {
        animator.SetBool(animIdClimb, state);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger(animIdAttack);
    }

    public void OnFootstep(AnimationEvent animationEvent)
    {
        soundEventChannel?.RaisePlaySFX("run");

        if (footstepClips.Length == 0 || animationEvent.animatorClipInfo.weight < 0.5f) return;

        var index = Random.Range(0, footstepClips.Length);
        Vector3 pos = transform.position + characterController.center;
        AudioSource.PlayClipAtPoint(footstepClips[index], pos, volume);
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        soundEventChannel?.RaisePlaySFX("land");
        
        if (animationEvent.animatorClipInfo.weight < 0.5f) return;

        Vector3 pos = transform.position + characterController.center;
        AudioSource.PlayClipAtPoint(landingClip, pos, volume);
    }
}