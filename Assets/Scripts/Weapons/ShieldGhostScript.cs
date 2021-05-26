using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGhostScript : GameEventListener
{
    [SerializeField] private Transform colliders;
    [SerializeField] private OnPlayerBossActionEvent onPlayerBossActionEvent;

    // Update is called once per frame
    void Update()
    {
        if (!CheckChildColliders())
            return;

        onPlayerBossActionEvent.Raise();
        Destroy(gameObject);
    }

    bool CheckChildColliders()
    {
        foreach (Transform child in colliders)
        {
            var shieldGhostCollider = child.gameObject.GetComponent<ShieldGhostCollider>();

            if (!shieldGhostCollider.IsTriggered)
                return false;
        }

        return true;
    }

    public void ActionExpired()
    {
        Destroy(gameObject);
    }
}
