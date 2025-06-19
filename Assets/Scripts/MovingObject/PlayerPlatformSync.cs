using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformSync : MonoBehaviour
{
    private Vector3 platformDelta = Vector3.zero;

    public void OnPlatformMoved(Vector3 delta)
    {
        platformDelta += delta;
    }

    public Vector3 ConsumePlatformDelta()
    {
        Vector3 result = platformDelta;
        platformDelta = Vector3.zero;
        return result;
    }
}

