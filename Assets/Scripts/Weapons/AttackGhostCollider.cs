using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGhostCollider : MonoBehaviour
{
    [HideInInspector] public bool IsTriggered { get; private set; }

    [SerializeField] private float triggerStayTime;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Hammer"))
            return;

        if (!IsTriggered)
            StartCoroutine(DisableTrigger());

        IsTriggered = true;
    }

    IEnumerator DisableTrigger()
    {
        yield return new WaitForSeconds(triggerStayTime);

        IsTriggered = false;
    }
}
