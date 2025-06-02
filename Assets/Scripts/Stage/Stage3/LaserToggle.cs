using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserToggle : MonoBehaviour
{
    [Header("Laser Settings")]
    public GameObject laserPrefab;        // Prefab of the laser beam (with MeshCollider)
    public Transform laserSpawnPoint;     // Where the laser should appear from
    public float onDuration = 2f;         // How long the laser stays on
    public float offDuration = 2f;        // How long the laser stays off

    private GameObject currentLaser;
    private bool isLaserOn = false;

    void Start()
    {
        StartCoroutine(ToggleLaserRoutine());
    }

    private System.Collections.IEnumerator ToggleLaserRoutine()
    {
        while (true)
        {
            if (!isLaserOn)
            {
                // Turn on laser
                currentLaser = Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation, transform);
                Vector3 newScale = currentLaser.transform.localScale;
                newScale.y = laserSpawnPoint.localScale.y;
                currentLaser.transform.localScale = newScale;
                isLaserOn = true;
                yield return new WaitForSeconds(onDuration);
            }
            else
            {
                // Turn off laser
                if (currentLaser != null)
                {
                    Destroy(currentLaser);
                }
                isLaserOn = false;
                yield return new WaitForSeconds(offDuration);
            }
        }
    }
}
