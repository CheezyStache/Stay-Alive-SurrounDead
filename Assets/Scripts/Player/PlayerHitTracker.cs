using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitTracker : MonoBehaviour
{
    [SerializeField] private float attackDelay;

    [SerializeField] private OnPlayerHitEvent onPlayerHit;

    private int enemyAttackCount;
    private List<int> enemyNames;

    void Start()
    {
        enemyAttackCount = 0;
        enemyNames = new List<int>();

        StartCoroutine(Hit());
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        if (enemyNames.Contains(collider.gameObject.GetInstanceID()))
            return;

        enemyAttackCount++;
        enemyNames.Add(collider.gameObject.GetInstanceID());
    }

    IEnumerator Hit()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);

            onPlayerHit.Raise(enemyAttackCount);
            enemyAttackCount = 0;
            enemyNames.Clear();
        }
    }
}
