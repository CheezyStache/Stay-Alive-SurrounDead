using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : GameEventListener
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _enemyParentTransform;
    [SerializeField] private int[] wavesCount;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _minSpawnDelay;
    [SerializeField] private float _spawnDelayDecreaseAmount;
    [SerializeField] private float _speedPerWaveIncrease;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private OnEnemyWaveEndedEvent onEnemyWaveEndedEvent;

    private int enemySpawnCount;

    public void StartNextWave()
    {
        enemySpawnCount = 0;

        foreach (Transform child in _enemyParentTransform)
            Destroy(child.gameObject);

        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        var waveCount = playerData.Wave >= wavesCount.Length ? wavesCount.Length - 1 : playerData.Wave;
        if (_spawnDelay > _minSpawnDelay)
            _spawnDelay -= _spawnDelayDecreaseAmount;

        while (enemySpawnCount < wavesCount[waveCount])
        {
            yield return new WaitForSeconds(_spawnDelay);

            if(playerData.Health <= 0)
                break;

            // Enemy initial position and rotation
            var position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
            var viewDirection = (_player.transform.position - position).normalized;
            var rotation = Quaternion.LookRotation(viewDirection);

            var enemy = _enemies[Random.Range(0, _enemies.Length)];
            var newEnemy = Instantiate(enemy, position, rotation, _enemyParentTransform);

            var enemyScript = newEnemy.GetComponent<EnemyScript>();
            if (enemyScript == null)
                throw new UnityException("Enemy doesn't have an EnemyScript");

            enemyScript.PlayerTransform = _player.transform;
            enemyScript.IncreaseSpeed(_speedPerWaveIncrease * playerData.Wave);
            enemySpawnCount++;
        }

        onEnemyWaveEndedEvent.Raise();
    }
}
