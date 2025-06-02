using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; set; }
    public bool MeleePressed { get; private set; }
    public bool RangedPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; set; }
    public bool SkillPressed { get; private set; }

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        float x = 0f;
        float y = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) y += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) y -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x -= 1;
        MoveInput = new Vector2(x, y).normalized;

        LookInput = mouse.delta.ReadValue();

        JumpPressed = keyboard.spaceKey.wasPressedThisFrame;
        MeleePressed = mouse.leftButton.wasPressedThisFrame;
        RangedPressed = mouse.rightButton.wasPressedThisFrame;
        DashPressed = keyboard.leftShiftKey.wasPressedThisFrame;
    }

    public bool TryConsumeJump()
    {
        if (JumpPressed)
        {
            JumpPressed = false;
            return true;
        }
        return false;
    }

    public void ConsumeDash() => DashPressed = false;
    public void ConsumeMelee() => MeleePressed = false;
    public void ConsumeRanged() => RangedPressed = false;

    public void ConsumeSkill() {}
}
