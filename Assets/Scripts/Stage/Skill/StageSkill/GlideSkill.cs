using UnityEngine;
using System.Collections;

public class GlideSkill : ISkill
{
    public IEnumerator Execute(SkillExecutionContext context)
    {
        context.RequestGlide?.Invoke();
        yield break;
    }
}
