using System;
using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    private float lastSkillTime = -Mathf.Infinity;

    private ISkill currentSkill;
    private SkillExecutionContext context;
    private PlayerInputReader inputReader;

    public event Action OnGlideRequested;
    public event Action OnTimeStopRequested;

    public void Initialize(ISkill skill, PlayerInputReader input)
    {
        currentSkill = skill;
        inputReader = input;

        Debug.Log($"[SkillController] Initialized with skill: {skill.GetType().Name}");

        context = new SkillExecutionContext(
            requestGlide: () => OnGlideRequested?.Invoke(),
            requestTimeStop: () => OnTimeStopRequested?.Invoke()
        );
    }

    void Update()
    {
        if (ShouldUseSkill())
        {
            lastSkillTime = Time.time;
            StartCoroutine(currentSkill.Execute(context));
            inputReader.ConsumeSkill();
        }
    }

    private bool ShouldUseSkill()
    {
        if (inputReader == null) return false;
        return inputReader.SkillPressed && (Time.time >= lastSkillTime + cooldownDuration);
    }
}
