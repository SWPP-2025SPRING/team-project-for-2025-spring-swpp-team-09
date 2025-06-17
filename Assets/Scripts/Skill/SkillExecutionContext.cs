using UnityEngine;

public class SkillExecutionContext
{
    public System.Action RequestGlide { get; }
    public System.Action RequestTimeStop { get; }
    public System.Action RequestWallWalk { get; }

    public SkillExecutionContext(System.Action requestGlide, System.Action requestTimeStop, System.Action requestWallWalk)
    {
        RequestGlide = requestGlide;
        RequestTimeStop = requestTimeStop;
        RequestWallWalk = requestWallWalk;
    }
}
