using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementState
{
    public float animationBlend;
    public float inputMagnitude;
    public bool grounded;
    public bool triggeredJump;
    public bool freeFalling;
    public bool climb;

    public MovementState(
        float animationBlend,
        float inputMagnitude,
        bool grounded,
        bool triggeredJump,
        bool freeFalling,
        bool climb)
    {
        this.animationBlend = animationBlend;
        this.inputMagnitude = inputMagnitude;
        this.grounded = grounded;
        this.triggeredJump = triggeredJump;
        this.freeFalling = freeFalling;
        this.climb = climb;
    }
}

