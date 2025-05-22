using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeStopSkill : ISkill
{
    public IEnumerator Execute(SkillExecutionContext context)
    {
        context.RequestTimeStop?.Invoke();
        yield break;
    }
}
