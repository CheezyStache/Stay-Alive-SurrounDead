using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _dieDelay;
    [SerializeField] private float _dieFallSpeed;
    [SerializeField] private float _dieFallY;

    [HideInInspector] public Transform PlayerTransform { get; set; }

    private Rigidbody _rigidbody;
    private Animator _animator;

    private bool isRunning = true;
    private bool died = false;

    private const string RunForwardAnimation = "Run Forward";
    private const string AttackAnimation = "Attack 01";
    private const string DieAnimation = "Die";

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
        if(died)
            return;

        if (collider.gameObject.tag != "Player")
            return;

        isRunning = false;
        ChangeAnimation(AttackAnimation);
    }

    void OnTriggerExit(Collider collider)
    {
        if (died)
            return;

        if (collider.gameObject.tag != "Player")
            return;

        isRunning = true;
        ChangeAnimation(RunForwardAnimation);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (died)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Hammer"))
            Die();
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

    private void Die()
    {
        isRunning = false;
        died = true;
        ChangeAnimation(DieAnimation);
        StartCoroutine(DieFall());
    }

    private IEnumerator DieFall()
    {
        yield return new WaitForSeconds(_dieDelay);

        _rigidbody.isKinematic = true;

        var time = 0f;
        var fallTime = (transform.position.y - _dieFallY) / _dieFallSpeed;

        while (time < fallTime)
        {
            time += Time.deltaTime;

            transform.Translate(Vector3.down * _dieFallSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(gameObject);
    }
}
