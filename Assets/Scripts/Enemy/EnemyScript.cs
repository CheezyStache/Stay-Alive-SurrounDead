using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _speed;

    [HideInInspector] public Transform PlayerTransform { get; set; }

    private Rigidbody _rigidbody;
    private Animator _animator;

    private const string RunForwardAnimation = "Run Forward";

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (PlayerTransform == null)
            return;

        _animator.SetBool(RunForwardAnimation, true);

        var step = Time.deltaTime * _speed;
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, PlayerTransform.position, step));
    }
}
