using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageGameManager gm = FindObjectOfType<StageGameManager>();
            if (gm != null)
            {
                gm.GameOver();
            }
        }
    }
}
