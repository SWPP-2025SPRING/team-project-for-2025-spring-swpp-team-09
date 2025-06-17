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
        Debug.Log($"[CollisionEventHandler] OnPlayerHit received: {col.name}");
        PlayerController player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log($"[CollisionEventHandler] ApplySlow to {col.name} with ratio={ratio}, duration={duration}");
            player.ApplySlow(ratio, duration);
        } else  Debug.LogWarning($"[CollisionEventHandler] {col.name} does not have PlayerController.");
    }
}
