using UnityEngine;

public class ObjectMovementController : MonoBehaviour
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
    public float spinSpeed = 0f; // Degrees per second

    private Vector3 startPos;
    private Transform cachedTransform;

    void Awake()
    {
        cachedTransform = transform;
    }

    void Start()
    {
        startPos = cachedTransform.position;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
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
}
