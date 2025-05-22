using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeStopSkill : ISkill
{
    public IEnumerator Execute(SkillExecutionContext context)
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            context.RequestTimeStop?.Invoke();
        }
        yield break;
    }
}
