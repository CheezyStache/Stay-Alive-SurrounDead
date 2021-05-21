using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnPlayerHit", menuName = "On Player Hit Event", order = 2)]
public class OnPlayerHitEvent : GameEvent
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private OnPlayerHealthChangeEvent onPlayerHealthChange;

    public void Raise(int damage)
    {
        playerData.Health -= damage;

        if (damage != 0)
        {
            onPlayerHealthChange.Raise(-damage);
            Raise();
        }
    }
}
