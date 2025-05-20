using UnityEngine;

public class WaterZone : MonoBehaviour
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