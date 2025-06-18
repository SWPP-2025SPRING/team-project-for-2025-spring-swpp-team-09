using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovablePlatform
{
    Vector3 DeltaMovement { get; }
    void RegisterPlatformSync(PlayerPlatformSync rider);
    void UnregisterPlatformSync(PlayerPlatformSync rider);
}