using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _dieDelay;
    [SerializeField] private float _dieFallSpeed;
    [SerializeField] private float _dieFallY;
    [SerializeField] private float _notLookDistance;

    [HideInInspector] public Transform PlayerTransform { get; set; }

    private Rigidbody _rigidbody;
    private Animator _animator;

    private EnemyState _state;

    private const string RunForwardAnimation = "Run Forward";
    private const string AttackAnimation = "Attack 01";
    private const string DieAnimation = "Die";

    void Start()
    {
        _state = EnemyState.Run;

        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _animator.SetBool(RunForwardAnimation, true);
    }

    void FixedUpdate()
    {
        if(_state == EnemyState.Run)
            Run();
    }

    void OnTriggerStay(Collider collider)
    {
        if (_state == EnemyState.Die)
            return;

        if (collider.gameObject.tag != "Player")
            return;

        _state = EnemyState.Attack;
        ChangeAnimation(AttackAnimation);
    }

    void OnTriggerExit(Collider collider)
    {
        if (_state != EnemyState.Attack)
            return;

        if (collider.gameObject.tag != "Player")
            return;

        _state = EnemyState.Run;
        ChangeAnimation(RunForwardAnimation);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_state == EnemyState.Die)
            return;

        if (collision.gameObject.layer != LayerMask.NameToLayer("Hammer"))
            return;

        _state = EnemyState.Die;
        ChangeAnimation(DieAnimation);
        StartCoroutine(DieFall());
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

    private IEnumerator DieFall()
    {
        _rigidbody.isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;

        var bottomPoint = new Vector3(transform.position.x, _dieFallY, transform.position.z);

        yield return new WaitForSeconds(_dieDelay);

        var time = 0f;
        var fallTime = Vector3.Distance(transform.position, bottomPoint) / _dieFallSpeed;

        while (time < fallTime)
        {
            time += Time.deltaTime;

            var direction = bottomPoint - transform.position;
            direction.Normalize();
            transform.Translate(direction * _dieFallSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(gameObject);
    }

    private enum EnemyState
    {
        Run,
        Attack,
        Die
    }
}
