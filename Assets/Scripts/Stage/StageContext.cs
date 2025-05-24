using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageContext
{
    public string StageId { get; }
    public ISkill Skill { get; }

    public StageContext(string stageId, ISkill skill)
    {
        StageId = stageId;
        Skill = skill;
    }
}
