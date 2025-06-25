using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IPlayerControlHandler
{
    public PlayerInputReader inputReader;
    public MovementController movementController;
    public AttackController attackController;
    public AnimationController animationController;
    public SkillController skillController;
    private PlayerPlatformSync platformSync;
    private StageGameManager stageGameManager;


    void Awake()
    {
        platformSync = GetComponent<PlayerPlatformSync>();
    }

    void Start()
    {
        animationController.Initialize();
        stageGameManager = FindObjectOfType<StageGameManager>();
    
        string stageId = GameFlowManager.Instance.GetStageContext()?.StageId;
        Debug.Log($"[PlayerController] Retrieved stageId: {stageId}");
        if (stageId != "Stage1")
        {
            skillController = GetComponent<SkillController>();
        }
    }

    private void Update()
    {
        if (stageGameManager != null && stageGameManager.IsPaused)
            return;

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
        skillController.Initialize(
            skill,
            inputReader,
            movementController,
            this,                
            skillController.NotifySkillEnded
        );
    }

    public void EnableInput(bool enabled)
    {
        if (inputReader != null)
            inputReader.inputEnabled = enabled;
    }
}
