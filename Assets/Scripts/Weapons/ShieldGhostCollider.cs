using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGhostCollider : MonoBehaviour
{
    [HideInInspector] public bool IsTriggered { get; private set; }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Shield"))
            return;

        IsTriggered = true;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Shield"))
            return;

        IsTriggered = false;
    }
}
