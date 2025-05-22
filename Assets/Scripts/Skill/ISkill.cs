using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    IEnumerator Execute(SkillExecutionContext context);
}

