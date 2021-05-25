using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : GameEventListener
{
    [HideInInspector] public GameObject Player { get; set; }

    [SerializeField] private float speed;
    [SerializeField] private float dieDelay;
    [SerializeField] private float dieFallSpeed;
    [SerializeField] private float dieFallY;
    [SerializeField] private float stopDistance;
    [SerializeField] private float actionTime;
    [SerializeField] private float health;

    [SerializeField] private OnPlayerWaveChangeEvent onPlayerWaveChangeEvent;
    [SerializeField] private OnPlayerHitEvent onPlayerHit;
    [SerializeField] private OnBossAttackEvent onBossAttackEvent;
    [SerializeField] private OnBossDefendEvent onBossDefendEvent;

    private Rigidbody _rigidbody;
    private Animator animator;

    private bool actionDone;

    private BossState state;

    private const string RunForwardAnimation = "Run Forward";
    private const string AttackAnimation = "Attack 01";
    private const string HitAnimation = "Take Damage";
    private const string DieAnimation = "Die";

    void Start()
    {
        state = BossState.Run;

        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        animator.SetBool(RunForwardAnimation, true);
    }

    void FixedUpdate()
    {
        if (state == BossState.Run)
            Run();
    }

    public void Stop()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        _rigidbody.isKinematic = true;
        state = BossState.Stop;
    }

    public void PlayerActionIsDone()
    {
        actionDone = true;
    }

    private void Run()
    {
        if (Player == null)
            return;

        var step = Time.deltaTime * speed;
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, Player.transform.position, step));

        if (Vector3.Distance(transform.position, Player.transform.position) <= stopDistance)
        {
            animator.SetBool(RunForwardAnimation, false);
            state = BossState.InBattle;
            Attack();
        }
    }

    private void Attack()
    {
        StartCoroutine(WaitAction());
    }

    IEnumerator WaitAction()
    {
        while (state == BossState.InBattle)
        {
            var randomAction = Random.Range(0, 10);
            var isAttack = randomAction % 2 == 0;

            if(isAttack)
                onBossAttackEvent.Raise();
            else
                onBossDefendEvent.Raise();

            yield return new WaitForSeconds(actionTime);

            if (actionDone && isAttack)
            {
                health--;

                if (health <= 0)
                    Die();
                else
                    ChangeAnimation(HitAnimation);
            }
            else if (!actionDone && !isAttack)
            {
                onPlayerHit.Raise();
                ChangeAnimation(AttackAnimation);
            }

            actionDone = false;
        }
    }

    private void ChangeAnimation(string nextAnimation)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
            animator.SetBool(parameter.name, false);

        animator.SetBool(nextAnimation, true);
    }

    private void Die()
    {
        state = BossState.Die;
        ChangeAnimation(DieAnimation);
        StartCoroutine(DieFall());
    }

    private IEnumerator DieFall()
    {
        _rigidbody.isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;

        var bottomPoint = new Vector3(transform.position.x, dieFallY, transform.position.z);

        yield return new WaitForSeconds(dieDelay);

        if (state == BossState.Stop)
            yield break;

        var time = 0f;
        var fallTime = Vector3.Distance(transform.position, bottomPoint) / dieFallSpeed;

        while (time < fallTime)
        {
            time += Time.deltaTime;

            var direction = bottomPoint - transform.position;
            direction.Normalize();
            transform.Translate(direction * dieFallSpeed * Time.deltaTime);

            yield return null;
        }

        onPlayerWaveChangeEvent.BossWaveFinished();
        Destroy(gameObject);
    }

    private enum BossState
    {
        Run,
        InBattle,
        Die,
        Stop
    }
}
