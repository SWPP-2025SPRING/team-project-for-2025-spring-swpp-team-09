using UnityEngine;

public class SkillExecutionContext
{
    public System.Action RequestGlide { get; }
    public System.Action RequestTimeStop { get; }

    public SkillExecutionContext(System.Action requestGlide, System.Action requestTimeStop)
    {
        RequestGlide = requestGlide;
        RequestTimeStop = requestTimeStop;
    }
}


