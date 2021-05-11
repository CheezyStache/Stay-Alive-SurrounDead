using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _speed;

    [HideInInspector] public Transform PlayerTransform { get; set; }

    private Rigidbody _rigidbody;
    private Animator _animator;

    private bool isRunning = true;

    private const string RunForwardAnimation = "Run Forward";
    private const string AttackAnimation = "Attack 01";

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _animator.SetBool(RunForwardAnimation, true);
    }

    void FixedUpdate()
    {
        if(isRunning)
            Run();
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag != "Player")
            return;

        isRunning = false;
        ChangeAnimation(AttackAnimation);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag != "Player")
            return;

        isRunning = true;
        ChangeAnimation(RunForwardAnimation);
    }


    private void Run()
    {
        if (PlayerTransform == null)
            return;

        var step = Time.deltaTime * _speed;
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, PlayerTransform.position, step));
    }

    private void ChangeAnimation(string nextAnimation)
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
            _animator.SetBool(parameter.name, false);

        _animator.SetBool(nextAnimation, true);
    }
}
