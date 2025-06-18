using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTopTrigger : MonoBehaviour
{
    private IMovablePlatform platform;

    void Start()
    {
        platform = GetComponentInParent<IMovablePlatform>();
    }

    void OnTriggerEnter(Collider other)
    {
        var sync = other.GetComponent<PlayerPlatformSync>();
        if (sync != null)
        {
            platform.RegisterPlatformSync(sync);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var sync = other.GetComponent<PlayerPlatformSync>();
        if (sync != null) platform.UnregisterPlatformSync(sync);
    }
}

