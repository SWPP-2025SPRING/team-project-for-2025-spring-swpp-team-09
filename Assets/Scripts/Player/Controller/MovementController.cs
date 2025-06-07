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

    [SerializeField] private Transform leftFootTransform;
    [SerializeField] private float footToWallOffset = 0.05f;

    [Header("Camera")]
    public GameObject cameraTarget;

    private CharacterController controller;
    private AnimationController animationController;
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

    public void ProcessMovement(PlayerInputReader input, out float animationBlend, out float inputMagnitude, out bool isGrounded, out bool triggerJump, out bool freeFall)
    {
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

        animationBlend = Mathf.Lerp(animationBlendPrev, targetSpeed, Time.deltaTime * speedChangeRate);
        animationBlend = animationBlend < 0.01f ? 0f : animationBlend;
        animationBlendPrev = animationBlend;

        Vector3 inputDir = new Vector3(move.x, 0f, move.y).normalized;
        if (inputDir != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTarget.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        Vector3 direction = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;

        triggerJump = false;
        JumpAndGravity(input, ref triggerJump);

        controller.Move((direction * currentSpeed + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (input.DashPressed && canDash)
        {
            Dash();
            input.ConsumeDash();
        }

        freeFall = !grounded && verticalVelocity < 0f;

        if (input.SkillPressed && CanWallWalk(out wallNormal))
        {
            StartCoroutine(WallWalkRoutine(wallNormal, input));
            input.ConsumeSkill(); // E.g., E 키를 소비
        }
        if (isWallWalking)
        {
            isGrounded = true; // Animator에 Grounded 전달용
            animationController?.SetGrounded(true);
        }

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
                jumpTimeoutDelta -= Time.deltaTime;

            hasDoubleJumped = false;
            canDoubleJump = true;
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta > 0f)
                fallTimeoutDelta -= Time.deltaTime;

            if (input.TryConsumeJump() && canDoubleJump && !hasDoubleJumped)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -4f * gravity);
                hasDoubleJumped = true;
                triggerJump = true;

            }
        }

        verticalVelocity += gravity * Time.deltaTime;
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

        if (!grounded && isWallWalking)
        {
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, wallLayer, QueryTriggerInteraction.Ignore);
        }
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

    public void StartWallWalk(PlayerInputReader input)
    {
        if (!isWallWalking && CanWallWalk(out Vector3 wallNormal))
        {
            StartCoroutine(WallWalkRoutine(wallNormal, input));
        }
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

        float timer = 0f;
        float stickForce = 2f;

        // 1. 회전 방향 설정
        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up).normalized;
        Vector3 upDir = Vector3.Cross(wallForward, -wallNormal); // 발 → 머리 방향
        Quaternion targetRotation = Quaternion.LookRotation(wallForward, upDir);
        transform.rotation = targetRotation;

        // 2. 발 기준으로 벽 정렬
        if (Physics.Raycast(leftFootTransform.position, -wallNormal, out RaycastHit hit, 1f, wallLayer))
        {
            // 벽면 방향으로의 수직 거리만 추출
            Vector3 footToWall = wallNormal * Vector3.Dot((hit.point - leftFootTransform.position), wallNormal);
            Vector3 correction = footToWall + wallNormal * footToWallOffset; // 약간 띄우기
            controller.Move(correction); // 충돌 대응 안전 이동
        }

        // 3. 루프: 이동, 밀착, 중력 제거, 애니메이션 처리
        while (timer < wallWalkDuration)
        {
            // 벽에 밀착
            Vector3 toWall = -wallNormal * stickForce * Time.deltaTime;
            controller.Move(toWall);

            // 입력 방향으로 벽 위 이동
            Vector2 move = input.MoveInput;
            Vector3 walkDir = (wallForward * move.y + upDir * move.x).normalized;
            controller.Move(walkDir * wallWalkSpeed * Time.deltaTime);

            // 중력 제거 + 애니메이션
            verticalVelocity = 0f;
            grounded = true;
            animationController?.SetGrounded(true);
            animationController?.UpdateMovement(wallWalkSpeed, move.magnitude);

            // Debug 시각화
            Debug.DrawRay(transform.position, -wallNormal * 0.5f, Color.red);

            timer += Time.deltaTime;
            yield return null;
        }

        // 종료 처리
        isWallWalking = false;
        animationController?.SetGrounded(false);
        animationController?.UpdateMovement(0f, 0f);
    }


    public float CurrentSpeed => moveSpeed;
}
