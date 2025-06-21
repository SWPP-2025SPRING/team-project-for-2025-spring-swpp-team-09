using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallWalkSkill : ISkill
{
    public IEnumerator Execute(SkillExecutionContext context)
    {
        context.movementController.StartWallWalk(context.inputReader);
        yield return null;
        context.onSkillEnded?.Invoke();
    }
}

