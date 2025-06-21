using System;
using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 20f;
    [SerializeField] private SoundEventChannel soundEventChannel;
    private SkillExecutionContext context;
    private float lastSkillTime = -Mathf.Infinity;
    private ISkill currentSkill;
    private PlayerInputReader inputReader;

    public MovementController movementController;
    public SkillCooldownUI cooldownUI;

    public void Initialize(
        ISkill skill,
        PlayerInputReader input,
        MovementController movement,
        MonoBehaviour invoker,
        Action onSkillEnded)
    {
        currentSkill = skill;
        inputReader = input;
        movementController = movement;

        context = new SkillExecutionContext(invoker, movement, input, onSkillEnded);
    }


    void Update()
    {
        if (ShouldUseSkill())
        {
            var context = new SkillExecutionContext(
                this, // invoker
                movementController,
                inputReader,
                NotifySkillEnded
            );

            StartCoroutine(currentSkill.Execute(context));
            inputReader.ConsumeSkill();
        }
    }

    private bool ShouldUseSkill()
    {
        return inputReader != null &&
               inputReader.SkillPressed &&
               Time.time >= lastSkillTime + cooldownDuration;
    }

    public void NotifySkillEnded()
    {
        lastSkillTime = Time.time;
        cooldownUI?.StartCooldown(cooldownDuration);
    }

    public bool CanUseSkill()
    {
        return Time.time >= lastSkillTime + cooldownDuration;
    }
}

