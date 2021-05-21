using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitTracker : MonoBehaviour
{
    [SerializeField] private float attackDelay;

    [SerializeField] private OnPlayerHitEvent onPlayerHit;

    private int enemyAttackCount;

    void Start()
    {
        enemyAttackCount = 0;

        StartCoroutine(Hit());
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        enemyAttackCount++;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        enemyAttackCount--;
    }

    IEnumerator Hit()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);

            onPlayerHit.Raise(enemyAttackCount);
        }
    }
}
