using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGhostScript : GameEventListener
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
        for (int i = colliders.childCount - 1; i >= 0; i--)
        {
            var attackGhostCollider = colliders.GetChild(i).gameObject.GetComponent<AttackGhostCollider>();

            if (!attackGhostCollider.IsTriggered)
                return false;
        }

        return true;
    }

    public void ActionExpired()
    {
        Destroy(gameObject);
    }
}
