using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool MeleePressed { get; private set; }
    public bool RangedPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; private set; }

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        float x = 0f;
        float y = 0f;
        if (keyboard.wKey.isPressed) y += 1;
        if (keyboard.sKey.isPressed) y -= 1;
        if (keyboard.dKey.isPressed) x += 1;
        if (keyboard.aKey.isPressed) x -= 1;
        MoveInput = new Vector2(x, y).normalized;

        LookInput = mouse.delta.ReadValue();

        JumpPressed = keyboard.spaceKey.wasPressedThisFrame;
        MeleePressed = mouse.leftButton.wasPressedThisFrame;
        RangedPressed = mouse.rightButton.wasPressedThisFrame;
    }

    public void ConsumeJump() => JumpPressed = false;
    public void ConsumeDash() => DashPressed = false;
    public void ConsumeMelee() => MeleePressed = false;
    public void ConsumeRanged() => RangedPressed = false;
}
