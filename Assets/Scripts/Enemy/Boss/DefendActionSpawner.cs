using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendActionSpawner : GameEventListener
{
    [SerializeField] private GameObject shieldGhost;
    [SerializeField] private float distance;
    [SerializeField] private float radius;
    [SerializeField] private float maxZ;
    [SerializeField] private float minY;
    [SerializeField] private float heightOffset;
    [SerializeField] private Transform player;

    public void Spawn()
    {
        var zPoint = player.position.z + distance > maxZ ? maxZ : player.position.z + distance;
        var yPoint = player.position.y - heightOffset;

        var randomX = Random.Range(-radius, radius);
        var randomY = Random.Range(-radius, radius);

        yPoint += randomY;
        if (yPoint < minY)
            yPoint = minY;

        Instantiate(shieldGhost, new Vector3(randomX, yPoint, zPoint), Quaternion.identity,
            gameObject.transform);
    }
}
