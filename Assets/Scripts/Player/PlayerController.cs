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
    private PlayerPlatformSync platformSync;

    [SerializeField] private GameObject followCamera;

    private bool timeStopped = false;

    void Awake()
    {
        platformSync = GetComponent<PlayerPlatformSync>();
    }

    void Start()
    {
        animationController.Initialize();
        skillController = GetComponent<SkillController>();

        skillController.OnTimeStopRequested += () =>
        {
            StartCoroutine(HandleTimeStop());
        };
        skillController.OnWallWalkRequested += HandleWallWalkRequested;
    }

    private void Update()
    {
        Vector3 platformDelta = platformSync != null ? platformSync.ConsumePlatformDelta() : Vector3.zero;

        attackController.HandleAttackInput(inputReader);

        movementController.ProcessMovement(inputReader, out float animBlend, out float inputMag, out bool grounded, out bool jumpTrig, out bool freeFall, out bool climb, platformDelta);

        animationController.UpdateMovement(animBlend, inputMag);
        if (jumpTrig) animationController.TriggerJump();
        animationController.SetGrounded(grounded);
        animationController.SetFreeFall(freeFall);
        animationController.SetClimb(climb);
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
            inputReader.inputEnabled = enabled;
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
        skillController.NotifySkillEnded();
    }
    private void HandleWallWalkRequested()
    {
        movementController.StartWallWalk(inputReader); // 다음 단계에서 구현할 메서드
    }
}
