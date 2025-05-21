using UnityEngine;

public class CollisionEventHandler : MonoBehaviour
{
    [SerializeField] private CollisionEventChannel collisionChannel;

    private void OnEnable()
    {
        if (collisionChannel != null)
            collisionChannel.OnPlayerHit.AddListener(HandlePlayerHit);
    }

    private void OnDisable()
    {
        if (collisionChannel != null)
            collisionChannel.OnPlayerHit.RemoveListener(HandlePlayerHit);
    }

    private void HandlePlayerHit(Collider col, float ratio, float duration)
    {
        PlayerController player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplySlow(ratio, duration);
        }
    }
}
