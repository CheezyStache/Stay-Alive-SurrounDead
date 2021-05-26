using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionSpawner : GameEventListener
{
    [SerializeField] private GameObject attackGhost;
    [SerializeField] private float distance;
    [SerializeField] private float radius;
    [SerializeField] private float maxZ;
    [SerializeField] private float minY;
    [SerializeField] private float heightOffset;
    [SerializeField] private Vector3 scale;
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

        attackGhost.transform.localScale = scale;
        var randomRotation = Random.Range(0f, 359f);
        var rotation = Quaternion.Euler(0f, 0f, randomRotation);

        Instantiate(attackGhost, new Vector3(randomX, yPoint, zPoint), rotation,
            gameObject.transform);
    }
}
