using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : GameEventListener
{
    [SerializeField] private float speed;
    [SerializeField] private float dieDelay;
    [SerializeField] private float dieFallSpeed;
    [SerializeField] private float dieFallY;
    [SerializeField] private float actionTime;
    [SerializeField] private float health;
    [SerializeField] private int damage;
    [SerializeField] private Vector3 goToPosition;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private int increaseHealthEveryWave;

    [SerializeField] private OnPlayerWaveChangeEvent onPlayerWaveChangeEvent;
    [SerializeField] private OnPlayerHitEvent onPlayerHit;
    [SerializeField] private OnBossAttackEvent onBossAttackEvent;
    [SerializeField] private OnBossDefendEvent onBossDefendEvent;
    [SerializeField] private OnPlayerFailedActionEvent onPlayerFailedAction;

    private Rigidbody _rigidbody;
    private Animator animator;

    private BossState state;
    private IEnumerator waitCoroutine;
    private bool isAttack;

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
        health += Mathf.Floor((float)playerData.Wave / (float)increaseHealthEveryWave);
        damage += Mathf.FloorToInt((float)playerData.Wave / (float)increaseHealthEveryWave);
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
        StopCoroutine(waitCoroutine);

        if (isAttack)
        {
            health--;

            if (health <= 0)
                Die();
            else
                ChangeAnimation(HitAnimation);
        }

        StartCoroutine(WaitAction());
    }

    private void Run()
    {
        var step = Time.deltaTime * speed;
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, goToPosition, step));

        if (Vector3.Distance(transform.position, goToPosition) <= 0.5f)
        {
            animator.SetBool(RunForwardAnimation, false);
            state = BossState.InBattle;
            StartCoroutine(WaitAction());
        }
    }

    IEnumerator WaitAction()
    {
        if(state != BossState.InBattle)
            yield break;

        var randomAction = Random.Range(0, 10);
        isAttack = randomAction % 2 == 0;

        if (isAttack)
            onBossAttackEvent.Raise();
        else
            onBossDefendEvent.Raise();

        waitCoroutine = ActionDone();
        StartCoroutine(waitCoroutine);
    }

    IEnumerator ActionDone()
    {
        yield return new WaitForSeconds(actionTime);

        onPlayerFailedAction.Raise();

        if (!isAttack)
        {
            onPlayerHit.Raise(damage);
            ChangeAnimation(AttackAnimation);
        }

        StartCoroutine(WaitAction());
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

        onPlayerWaveChangeEvent.NextWave();
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
