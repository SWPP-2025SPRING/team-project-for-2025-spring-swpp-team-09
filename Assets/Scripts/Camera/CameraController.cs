using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followTarget;
    public float topClamp = 70f;
    public float bottomClamp = -30f;
    public float cameraAngleOverride = 0f;
    public bool lockCameraPosition = false;

    [SerializeField] private PlayerInputReader input; // 주입 방식으로 교체

    private float yaw;
    private float pitch;

    private const float threshold = 0.01f;

    void LateUpdate()
    {
        if (input.LookInput.sqrMagnitude >= threshold && !lockCameraPosition)
        {
            float deltaMultiplier = 1f;
            yaw += input.LookInput.x * deltaMultiplier;
            pitch += input.LookInput.y * deltaMultiplier;
        }

        yaw = ClampAngle(yaw, float.MinValue, float.MaxValue);
        pitch = ClampAngle(pitch, bottomClamp, topClamp);

        followTarget.transform.rotation = Quaternion.Euler(pitch + cameraAngleOverride, yaw, 0f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
