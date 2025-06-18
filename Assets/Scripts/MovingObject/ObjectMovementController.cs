using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementController : MonoBehaviour, IMovablePlatform
{
    public enum MoveDirection
    {
        None,
        UpDown,
        LeftRight,
        RightLeft
    }

    [Header("Movement Settings")]
    public MoveDirection moveDirection = MoveDirection.None;
    public float moveDistance = 1.0f;
    public float moveSpeed = 1.0f;

    [Header("Rotation Settings")]
    public bool enableRotation = false;
    public float spinSpeed = 0f;

    private Vector3 startPos;
    private Vector3 lastPosition;
    private Vector3 deltaMovement;
    private Transform cachedTransform;

    private PlayerPlatformSync rider;

    public Vector3 DeltaMovement => deltaMovement;

    void Awake()
    {
        cachedTransform = transform;
    }

    void Start()
    {
        startPos = cachedTransform.position;
        lastPosition = startPos;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();

        deltaMovement = cachedTransform.position - lastPosition;
        lastPosition = cachedTransform.position;

        if (rider != null)
        {
            rider.OnPlatformMoved(deltaMovement);
        }
    }

    private void HandleMovement()
    {
        if (moveDirection == MoveDirection.None) return;

        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        switch (moveDirection)
        {
            case MoveDirection.UpDown:
                cachedTransform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
                break;
            case MoveDirection.LeftRight:
                cachedTransform.position = new Vector3(startPos.x - offset, startPos.y, startPos.z);
                break;
            case MoveDirection.RightLeft:
                cachedTransform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
                break;
        }
    }

    private void HandleRotation()
    {
        if (!enableRotation) return;

        cachedTransform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    public void RegisterPlatformSync(PlayerPlatformSync sync)
    {
        rider = sync;
    }

    public void UnregisterPlatformSync(PlayerPlatformSync sync)
    {
        if (rider == sync)
            rider = null;
    }
}
