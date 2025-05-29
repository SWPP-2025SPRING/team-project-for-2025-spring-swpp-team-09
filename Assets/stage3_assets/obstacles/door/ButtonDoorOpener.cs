using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorOpener : MonoBehaviour
{
    [Header("Door Settings")]
    public GameObject door;
    public float openDistance = 3f;
    public float openSpeed = 2f;

    [Header("Prompt Settings")]
    public GameObject promptUI;  // Assign a UI element like a Text or Canvas group

    private bool isPlayerNear = false;
    private bool isOpening = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        if (door != null)
        {
            initialPosition = door.transform.position;
            targetPosition = initialPosition + Vector3.up * openDistance;
        }

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check for "F" key when player is near
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            isOpening = true;
            if (promptUI != null)
                promptUI.SetActive(false);
        }

        // Move door
        if (isOpening && door != null)
        {
            door.transform.position = Vector3.MoveTowards(
                door.transform.position,
                targetPosition,
                openSpeed * Time.deltaTime
            );
        }
    }

    // Show prompt when player enters trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    // Hide prompt when player exits trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}
