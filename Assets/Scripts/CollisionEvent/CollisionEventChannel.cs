using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/CollisionEventChannel")]
public class CollisionEventChannel : ScriptableObject
{
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collider, float, float> { }

    public CollisionEvent OnPlayerHit = new();
    
    public void RaisePlayerHit(Collider playerCollider, float slowRatio, float duration)
    {
        OnPlayerHit.Invoke(playerCollider, slowRatio, duration);
    }
}
