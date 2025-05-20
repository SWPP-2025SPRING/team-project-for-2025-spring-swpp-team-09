using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputReader inputReader;
    public MovementController movementController;
    public AttackController attackController;
    public AnimationController animationController;

    private void Update()
    {
        attackController.HandleAttackInput(inputReader);
        
        movementController.ProcessMovement(inputReader, out float animBlend, out float inputMag, out bool grounded, out bool jumpTrig, out bool freeFall);

        animationController.UpdateMovement(animBlend, inputMag);
        animationController.SetGrounded(grounded);
        if (jumpTrig) animationController.TriggerJump();
        animationController.SetFreeFall(freeFall);
    }

    public void ApplySlow(float ratio, float duration)
    {
        movementController.ApplySpeedModifier(ratio, duration);
    }
}
