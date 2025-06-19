using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float sprintSpeed = 15f;
    public float rotationSmoothTime = 0.12f;
    public float speedChangeRate = 10f;
    private float speedMultiplier = 1f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -15f;
    public float terminalVelocity = -53f;
    public float jumpTimeout = 0.5f;
    public float fallTimeout = 0.15f;

    [Header("Ground Check")]
    public float groundedOffset = 0.14f;
    public float groundedRadius = 0.15f;
    public LayerMask groundLayers;

    [Header("Dash")]
    public float dashDistance = 6f;
    public float dashCooldown = 5f;
    private bool canDash = true;

    [Header("Glide")]
    private float glideTimeRemaining = 0f;
    [SerializeField] private float maxGlideTime = 3.0f;
    private bool isGliding = false;

    [Header("Wall Walk")]
    public float wallWalkDuration = 5f;
    public float wallWalkSpeed = 5f;
    public float wallCheckDistance = 1f;
    public LayerMask wallLayer;
    private bool isWallWalking = false;
    private Vector3 wallNormal;

    [Header("Camera")]
    public GameObject cameraTarget;

    private CharacterController controller;
    public SkillController skillcontroller;
    private float verticalVelocity;
    private float targetRotation;
    private float rotationVelocity;
    private float currentSpeed;
    private float animationBlendPrev;
    private bool grounded;
    private bool wasGrounded;
 
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private bool canDoubleJump = true;
    private bool hasDoubleJumped = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    public void ProcessMovement(PlayerInputReader input, out float animationBlend, out float inputMagnitude, out bool isGrounded, out bool triggerJump, out bool freeFall, out bool climb, Vector3 platformDelta)
    {
        if (platformDelta.magnitude > 0.001f)
        {
            controller.Move(platformDelta);
        }

        GroundedCheck();
        bool justLanded = !wasGrounded && grounded;
        wasGrounded = grounded;

        if (justLanded)
        {
            hasDoubleJumped = false;
        }

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

        animationBlend = Mathf.Lerp(animationBlendPrev, targetSpeed, Time.unscaledDeltaTime * speedChangeRate);
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
                rotationSmoothTime,
                Mathf.Infinity,
                Time.unscaledDeltaTime
            );

            if (!float.IsNaN(rotation))
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }
        Vector3 direction = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;

        climb = isWallWalking;
        
        
        if (input.SkillPressed && CanWallWalk(out wallNormal) && skillcontroller.CanUseSkill())
        {
            StartCoroutine(WallWalkRoutine(wallNormal, input));
            input.ConsumeSkill();
            Debug.Log($"WallWalkRoutine started. Wall normal: {wallNormal}");
        }
        
        if (climb)
        {
            // 벽 기준 방향 재설정
            Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up).normalized;
            Vector3 wallUp = Vector3.Cross(wallForward, wallNormal).normalized;

            direction = (wallForward * move.x + wallUp * move.y) / 4;

            transform.rotation = Quaternion.LookRotation(-wallNormal);
        }

        triggerJump = false;
        JumpAndGravity(input, ref triggerJump);

        controller.Move((direction * currentSpeed + Vector3.up * verticalVelocity) * Time.unscaledDeltaTime);

        if (input.DashPressed && canDash)
        {
            Dash();
            input.ConsumeDash();
        }

        freeFall = !grounded && verticalVelocity < 0f;
    }

    private void JumpAndGravity(PlayerInputReader input, ref bool triggerJump)
    {
        if (isGliding) return;
        if (grounded)
        {
            fallTimeoutDelta = fallTimeout;

            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (input.TryConsumeJump() && jumpTimeoutDelta <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                triggerJump = true;

                glideTimeRemaining = maxGlideTime;
            }

            if (jumpTimeoutDelta > 0f)
                jumpTimeoutDelta -= Time.unscaledDeltaTime;

            hasDoubleJumped = false;
            canDoubleJump = true;
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta > 0f)
                fallTimeoutDelta -= Time.unscaledDeltaTime;

            if (input.TryConsumeJump() && canDoubleJump && !hasDoubleJumped)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -4f * gravity);
                hasDoubleJumped = true;
                triggerJump = true;

            }
        }

        verticalVelocity += gravity * Time.unscaledDeltaTime;
        verticalVelocity = Mathf.Max(verticalVelocity, terminalVelocity);
    }

    private void Dash()
    {
        Vector3 dashDir = transform.forward;
        controller.Move(dashDir.normalized * dashDistance);
        canDash = false;
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void ResetDash() => canDash = true;

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
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

    public void ActivateGlide()
    {
        if (!controller.isGrounded && !isGliding)
        {
            StartCoroutine(GlideRoutine());
        }
    }

    public void StartWallWalk(PlayerInputReader input)
    {
        if (!isWallWalking && CanWallWalk(out Vector3 wallNormal))
        {
            StartCoroutine(WallWalkRoutine(wallNormal, input));
            input.ConsumeSkill();
        }
    }

    private IEnumerator GlideRoutine()
    {
        isGliding = true;

        float duration = 3f;
        float timer = 0f;

        while (timer < duration)
        {
            if (controller.isGrounded) break;

            Debug.Log($"[Glide] verticalVelocity before: {verticalVelocity}");
            verticalVelocity = Mathf.Max(verticalVelocity, 3f);
            Debug.Log($"[Glide] verticalVelocity after: {verticalVelocity}");

            timer += Time.deltaTime;
            yield return null;
        }

        isGliding = false;
    }

    private bool CanWallWalk(out Vector3 normal)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, wallCheckDistance))
        {
            normal = hit.normal;
            return true;
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, wallCheckDistance))
        {
            normal = hit.normal;
            return true;
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance))
        {
            normal = hit.normal;
            return true;
        }
        normal = Vector3.zero;
        return false;
    }

    private IEnumerator WallWalkRoutine(Vector3 wallNormal, PlayerInputReader input)
    {
        isWallWalking = true;
        Debug.Log("WallWalkRoutine: Started");

        float timer = 0f;
        float stickForce = 2f; // 벽 쪽으로 밀어붙이는 정도
        float wallWalkSpeed = 0.1f;

        // 1. 안전한 wallRight 설정
        Vector3 wallRight = Vector3.Cross(Vector3.up, wallNormal);
        if (wallRight.magnitude < 0.01f)
            wallRight = Vector3.Cross(Vector3.forward, wallNormal);
        wallRight.Normalize();

        // 2. wallUp 생성
        Vector3 wallUp = Vector3.Cross(wallNormal, wallRight).normalized;

        // 3. 회전 생성
        Quaternion wallRotation = Quaternion.LookRotation(-wallNormal, wallUp);

        while (timer < wallWalkDuration)
        {
            if (!IsStillOnSameWall(wallNormal))
            {
                Debug.Log("벽에서 떨어져서 벽타기 종료");
                isWallWalking = false;
                break;
            }
            
            // 매 프레임 벽 기준으로 회전 고정
            transform.rotation = wallRotation;
            
            // ↓ 입력 처리: 아래 방향(S키)만
            Vector2 move = input.MoveInput;
            Vector3 moveDir = (wallRight * move.x + wallUp * move.y).normalized;

            if (moveDir != Vector3.zero)
            {
                controller.Move(moveDir * wallWalkSpeed * Time.deltaTime);
            }

            // 중력 제거
            verticalVelocity = 0f;

            timer += Time.deltaTime;
            yield return null;
        }

    isWallWalking = false;
    skillcontroller.NotifySkillEnded();
    }

    private bool IsStillOnSameWall(Vector3 originalWallNormal)
    {
        if (CanWallWalk(out Vector3 currentNormal))
        {
            float angle = Vector3.Angle(originalWallNormal, currentNormal);
            return angle < 15f; // 충분히 같은 방향이면 유지
        }
        return false;
    }

    public float CurrentSpeed => moveSpeed;
}
