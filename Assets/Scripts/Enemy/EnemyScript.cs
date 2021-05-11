using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _speed;

    [HideInInspector] public Transform PlayerTransform { get; set; }

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (PlayerTransform == null)
            return;

        var step = Time.deltaTime * _speed;
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, PlayerTransform.position, step));
    }
}
