using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputReader inputReader;
    public MovementController movementController;
    public AttackController attackController;
    public AnimationController animationController;

    private void Update()
    {
        movementController.ProcessMovement(inputReader);
        attackController.HandleAttackInput(inputReader);
        animationController.UpdateAnimation(movementController.CurrentSpeed);
    }

    public void ApplySlow(float ratio, float duration)
    {
        movementController.ApplySpeedModifier(ratio, duration);
    }
}
