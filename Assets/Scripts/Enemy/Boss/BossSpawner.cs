using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : GameEventListener
{
    [SerializeField] private GameObject boss;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject portal;
    [SerializeField] private Vector3 portalPosition;
    [SerializeField] private Quaternion portalRotation;

    [SerializeField] private OnPlayerWaveChangeEvent onPlayerWaveChangeEvent;

    public void ClearBoss()
    {
        foreach (Transform child in spawnPoint)
            Destroy(child.gameObject);
    }

    public void SpawnBoss()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnDelay);

        var portalInstance = Instantiate(portal, portalPosition, portalRotation, gameObject.transform);
        portalInstance.transform.localPosition = portalPosition;

        var viewDirection = (player.transform.position - transform.position).normalized;
        var rotation = Quaternion.LookRotation(viewDirection);

        Instantiate(boss, spawnPoint.position, rotation, spawnPoint);
    }
}
