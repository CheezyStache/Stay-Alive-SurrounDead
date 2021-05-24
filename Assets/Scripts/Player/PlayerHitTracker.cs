using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitTracker : GameEventListener
{
    [SerializeField] private float attackDelay;

    [SerializeField] private OnPlayerHitEvent onPlayerHit;
    [SerializeField] private PlayerData playerData;

    private int enemyAttackCount;
    private List<int> enemyNames;
    private bool immortal;

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

    public void EnableImmortality()
    {
        StartCoroutine(ImmortalityTimer());
    }

    IEnumerator Hit()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);

            if (playerData.Health <= 0 || immortal)
            {
                enemyAttackCount = 0;
                enemyNames.Clear();
                continue;
            }

            onPlayerHit.Raise(enemyAttackCount);
            enemyAttackCount = 0;
            enemyNames.Clear();
        }
    }

    IEnumerator ImmortalityTimer()
    {
        immortal = true;

        yield return new WaitForSeconds(attackDelay);

        immortal = false;
    }
}
