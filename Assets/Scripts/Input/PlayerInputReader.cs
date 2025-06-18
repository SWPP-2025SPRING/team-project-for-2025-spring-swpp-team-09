using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; set; }
    public bool MeleePressed { get; set; }
    public bool RangedPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; set; }
    public bool SkillPressed { get; set; }
    public bool PausePressed { get; set; }
    public bool testing = false;
    private bool _inputEnabled = true;
    public bool inputEnabled
    {
        get => _inputEnabled;
        set
        {
            _inputEnabled = value;

            if (!value)
            {
                MoveInput = Vector2.zero;
                LookInput = Vector2.zero;
                JumpPressed = false;
                MeleePressed = false;
                RangedPressed = false;
                DashPressed = false;
                SkillPressed = false;
            }
        }
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (!inputEnabled || testing) return;

        PausePressed = keyboard.escapeKey.wasPressedThisFrame;        

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
        SkillPressed = keyboard.eKey.wasPressedThisFrame;
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
    public void ConsumeSkill() => SkillPressed = false;
    public void ConsumePause() => PausePressed = false;
}
