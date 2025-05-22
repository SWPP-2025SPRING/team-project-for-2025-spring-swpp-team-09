using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/CombatEventChannel")]
public class CombatEventChannel : ScriptableObject
{
    public event Action<Collider, int> OnAttack;

    public void RaiseAttack(Collider target, int damage)
    {
        OnAttack?.Invoke(target, damage);
    }
}
