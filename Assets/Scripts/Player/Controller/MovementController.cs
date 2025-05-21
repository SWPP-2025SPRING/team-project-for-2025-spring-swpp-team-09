using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float sprintSpeed = 15f;
    public float rotationSmoothTime = 0.12f;
    public float gravity = -15f;
    public float jumpHeight = 1.2f;
    public float accelerationRate = 10f;

    [Header("Double Jump")]
    public float jumpTimeout = 0.5f;
    public float fallTimeout = 0.15f;

    [Header("Dash")]
    public float dashDistance = 6f;
    public float dashCooldown = 5f;
    private bool canDash = true;

    [Header("Ground Check")]
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.28f;
    public LayerMask groundLayers;

    [Header("Camera")]
    public GameObject cameraTarget;

    private CharacterController controller;
    private float verticalVelocity;
    private float rotationVelocity;
    private float targetRotation;
    private float speedMultiplier = 1f;
    private float currentSpeed;
    private float animationBlendPrev;
    private bool grounded;
    private bool canDoubleJump = true;
    private bool hasDoubleJumped = false;
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void ProcessMovement(PlayerInputReader input, out float animationBlend, out float inputMagnitude, out bool isGrounded, out bool triggerJump, out bool freeFall)
    {
        GroundedCheck();
        isGrounded = grounded;

        Vector2 move = input.MoveInput;
        inputMagnitude = move.magnitude;

        float targetSpeed = input.SprintHeld ? sprintSpeed : moveSpeed;
        targetSpeed *= speedMultiplier;
        if (move == Vector2.zero) targetSpeed = 0f;

        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            currentSpeed = targetSpeed * inputMagnitude;
        else
            currentSpeed = targetSpeed;

        animationBlend = Mathf.Lerp(animationBlendPrev, targetSpeed, Time.deltaTime * accelerationRate);
        animationBlend = animationBlend < 0.01f ? 0f : animationBlend;
        animationBlendPrev = animationBlend;

        Vector3 inputDir = new Vector3(move.x, 0f, move.y).normalized;
        if (inputDir != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTarget.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetRotation,
                ref rotationVelocity,
                rotationSmoothTime
            );
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        Vector3 direction = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;

        ApplyGravity();

        triggerJump = false;
        HandleJump(input, ref triggerJump);

        controller.Move((direction * currentSpeed + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (input.DashPressed && canDash)
        {
            Dash();
            input.ConsumeDash();
        }

        freeFall = !grounded && verticalVelocity < 0f;
    }

    private void HandleJump(PlayerInputReader input, ref bool triggerJump)
    {
        if (grounded)
        {
            fallTimeoutDelta = fallTimeout;

            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (input.JumpPressed && jumpTimeoutDelta <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                input.ConsumeJump();
                triggerJump = true;
                hasDoubleJumped = false;
            }

            if (jumpTimeoutDelta > 0f)
                jumpTimeoutDelta -= Time.deltaTime;

            canDoubleJump = true;
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta > 0f)
                fallTimeoutDelta -= Time.deltaTime;

            if (input.JumpPressed && canDoubleJump && !hasDoubleJumped)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -4f * gravity);
                input.ConsumeJump();
                triggerJump = true;
                hasDoubleJumped = true;
            }

            input.ConsumeJump();
        }
    }

    private void Dash()
    {
        Vector3 dashDir = transform.forward;
        controller.Move(dashDir.normalized * dashDistance);
        canDash = false;
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void ResetDash() => canDash = true;

    private void ApplyGravity()
    {
        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(spherePosition, Vector3.down * 0.1f, grounded ? Color.green : Color.red, 0.1f);
        Debug.Log($"[GroundedCheck] Position: {spherePosition}, Radius: {groundedRadius}, Grounded: {grounded}");
    }

    public void ApplySpeedModifier(float ratio, float duration)
    {
        StartCoroutine(SpeedModifierCoroutine(ratio, duration));
    }

    private IEnumerator SpeedModifierCoroutine(float ratio, float duration)
    {
        speedMultiplier = ratio;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
    }

    public float CurrentSpeed => moveSpeed * speedMultiplier;
}
