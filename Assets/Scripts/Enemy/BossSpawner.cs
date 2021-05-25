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

    [SerializeField] private OnPlayerWaveChangeEvent onPlayerWaveChangeEvent;
    private void Start()
    {
        onPlayerWaveChangeEvent.ResetWaves();
    }

    public void ClearBoss()
    {
        portal.SetActive(false);

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

        portal.SetActive(true);

        var viewDirection = (player.transform.position - transform.position).normalized;
        var rotation = Quaternion.LookRotation(viewDirection);

        var newBoss = Instantiate(boss, spawnPoint.position, rotation, spawnPoint);

        var bossScript = newBoss.GetComponent<BossScript>();
        if (bossScript == null)
            throw new UnityException("Boss doesn't have an BossScript");

        bossScript.Player = player;
    }
}
