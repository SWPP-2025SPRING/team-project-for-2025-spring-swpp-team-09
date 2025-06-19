using System;
using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 3f;
    private float lastSkillTime = -Mathf.Infinity;

    private ISkill currentSkill;
    private SkillExecutionContext context;
    private PlayerInputReader inputReader;
    public MovementController movementController;
    public SkillCooldownUI cooldownUI;
    public event Action OnGlideRequested;
    public event Action OnTimeStopRequested;
    public event Action OnWallWalkRequested;

    public void Initialize(ISkill skill, PlayerInputReader input)
    {
        currentSkill = skill;
        inputReader = input;

        Debug.Log($"[SkillController] Initialized with skill: {skill.GetType().Name}");

        context = new SkillExecutionContext(
            requestGlide: () => OnGlideRequested?.Invoke(),
            requestTimeStop: () => OnTimeStopRequested?.Invoke(),
            requestWallWalk: () => OnWallWalkRequested?.Invoke()
        );

        OnWallWalkRequested += () =>
        {
            movementController.StartWallWalk(inputReader);
        };
    }

    void Update()
    {
        Debug.Log($"[SkillController] Update - Time: {Time.time:F2}, LastSkillTime: {lastSkillTime:F2}, CooldownDuration: {cooldownDuration:F2}, (Time - LastSkillTime): {(Time.time - lastSkillTime):F2}, CanUse: {(Time.time >= lastSkillTime + cooldownDuration)}");
        
        if (ShouldUseSkill())
        {
            StartCoroutine(currentSkill.Execute(context));
            inputReader.ConsumeSkill();
        }
    }

    private bool ShouldUseSkill()
    {
        if (inputReader == null) return false;
        return inputReader.SkillPressed && (Time.time >= lastSkillTime + cooldownDuration);
    }

    public void NotifySkillEnded()
    {
        lastSkillTime = Time.time;
        cooldownUI?.StartCooldown(cooldownDuration);
        Debug.Log($"[SkillController] Skill ended, cooldown started at: {lastSkillTime:F2}");
    }

    public bool CanUseSkill()
    {
        return (Time.time >= lastSkillTime + cooldownDuration);
    }

}
