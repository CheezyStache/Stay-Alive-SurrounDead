using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _distance;
    [SerializeField] private float _yOffset;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }


    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);

            // Enemy initial position and rotation
            var randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            var position = randomDirection * _distance;
            position.y = _yOffset;
            var viewDirection = (_player.transform.position - position).normalized;
            var rotation = Quaternion.LookRotation(viewDirection);

            var enemy = _enemies[Random.Range(0, _enemies.Length)];
            var newEnemy = Instantiate(enemy, position, rotation, gameObject.transform);

            var enemyScript = newEnemy.GetComponent<EnemyScript>();
            if (enemyScript == null)
                throw new UnityException("Enemy doesn't have an EnemyScript");

            enemyScript.PlayerTransform = _player.transform;
        }
    }
}
