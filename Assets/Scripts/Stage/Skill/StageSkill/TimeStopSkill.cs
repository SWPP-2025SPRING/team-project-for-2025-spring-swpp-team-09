using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeStopSkill : ISkill
{
    public IEnumerator Execute(SkillExecutionContext context)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        context.onSkillEnded?.Invoke();
    }
}

