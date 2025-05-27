using UnityEngine;
using System.Collections;

// 카메라 관련 로직은 디자인 패턴대로 수정 필요 + StageGameManager
public class PlayerController : MonoBehaviour, IPlayerControlHandler
{
    public PlayerInputReader inputReader;
    public MovementController movementController;
    public AttackController attackController;
    public AnimationController animationController;
    public SkillController skillController;

    [SerializeField] private GameObject followCamera;

    private bool timeStopped = false;

    void Start()
    {
        animationController.Initialize();
        skillController = GetComponent<SkillController>();
        skillController.OnGlideRequested += () =>
        {
            movementController.ActivateGlide();
        };

        skillController.OnTimeStopRequested += () =>
        {
            StartCoroutine(HandleTimeStop());
        };
    }

    private void Update()
    {
        attackController.HandleAttackInput(inputReader);

        movementController.ProcessMovement(inputReader, out float animBlend, out float inputMag, out bool grounded, out bool jumpTrig, out bool freeFall);

        animationController.UpdateMovement(animBlend, inputMag);
        if (jumpTrig) animationController.TriggerJump();
        animationController.SetGrounded(grounded);
        animationController.SetFreeFall(freeFall);
    }

    public void ApplySlow(float ratio, float duration)
    {
        movementController.ApplySpeedModifier(ratio, duration);
    }

    public void SetSkill(ISkill skill)
    {
        skillController.Initialize(skill, inputReader);
    }

    public void EnableInput(bool enabled)
    {
        if (inputReader != null)
            inputReader.enabled = enabled;
    }

    public void LockCamera(bool isLocked)
    {
        if (followCamera != null)
            followCamera.SetActive(!isLocked);
    }

    /*
    private IEnumerator HandleTimeStop()
    {
        Debug.Log("time stop");
        timeStopped = true;

        foreach (var rb in FindObjectsOfType<Rigidbody>())
        {
            if (!rb.CompareTag("Player"))
                rb.isKinematic = true;
        }

        yield return new WaitForSecondsRealtime(3f);

        foreach (var rb in FindObjectsOfType<Rigidbody>())
        {
            if (!rb.CompareTag("Player"))
                rb.isKinematic = false;
        }

        timeStopped = false;
    }*/
    private IEnumerator HandleTimeStop()
    {
        Debug.Log("TimeStop: begin");
        timeStopped = true;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        timeStopped = false;
        Debug.Log("TimeStop: end");
    }
}
