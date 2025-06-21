using UnityEngine;
using System;

public class SkillExecutionContext
{
    public readonly MonoBehaviour invoker;
    public readonly MovementController movementController;
    public readonly PlayerInputReader inputReader;
    public readonly Action onSkillEnded;

    public SkillExecutionContext(
        MonoBehaviour invoker,
        MovementController movementController,
        PlayerInputReader inputReader,
        Action onSkillEnded
    )
    {
        this.invoker = invoker;
        this.movementController = movementController;
        this.inputReader = inputReader;
        this.onSkillEnded = onSkillEnded;
    }
}

