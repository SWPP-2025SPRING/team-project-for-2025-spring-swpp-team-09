using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject followTarget;

    [Header("Clamp Angles")]
    public float topClamp = 70f;
    public float bottomClamp = -30f;
    public float cameraAngleOverride = 0f;
    public bool lockCameraPosition = false;

    [Header("Sensitivity")]
    public float sensitivity = 70f;

    [SerializeField] private PlayerInputReader input;

    private float yaw;
    private float pitch;

    private const float threshold = 0.01f;

    void LateUpdate()
    {
        if (!Application.isFocused) return;

        if (input.LookInput.sqrMagnitude >= threshold && !lockCameraPosition)
        {
            float delta = sensitivity * Time.deltaTime;
            yaw += input.LookInput.x * delta;
            pitch -= input.LookInput.y * delta;

            pitch = ClampAngle(pitch, bottomClamp, topClamp);
        }

        yaw = ClampAngle(yaw, float.MinValue, float.MaxValue);
        followTarget.transform.rotation = Quaternion.Euler(pitch + cameraAngleOverride, yaw, 0f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
