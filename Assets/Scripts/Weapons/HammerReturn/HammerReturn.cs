using System;
using System.Collections;
using System.Collections.Generic;
using HammerState;
using UnityEngine;

public class HammerReturn : MonoBehaviour
{
    [SerializeField] private GameObject _hand;

    [SerializeField] private float _waitSeconds;
    [SerializeField] private float _flyHeight;
    [SerializeField] private float _flySpeed;
    [SerializeField] private float _returnTime;
    [SerializeField] private float _isNearRadius;

    private bool _hasTouchedGround;
    private IHammerState _state;
    private HammerState _stateEnum;

    // Start is called before the first frame update
    void Start()
    {
        _state = new IdleState(gameObject);
        _stateEnum = HammerState.Idle;

        GetComponent<Rigidbody>().sleepThreshold = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_state.StateUpdate())
            return;

        if (_stateEnum == HammerState.Fly)
        {
            _stateEnum = HammerState.Return;
            _state = new ReturnState(gameObject, _returnTime, _hand);
            return;
        }

        if (_stateEnum == HammerState.Return)
        {
            _stateEnum = HammerState.Idle;
            _state = new IdleState(gameObject);
            return;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (_stateEnum != HammerState.Idle || !_state.CheckCollision(collision) || _hasTouchedGround)
            return;

        _hasTouchedGround = true;
        StartCoroutine(CheckDistance());
    }

    private IEnumerator CheckDistance()
    {
        var position = new Vector3(transform.position.x, 0f, transform.position.z);
        var handPosition = new Vector3(_hand.transform.position.x, 0f, _hand.transform.position.z);
        var distance = Vector3.Distance(position, handPosition);

        if (distance <= _isNearRadius)
        {
            _hasTouchedGround = false;
            yield break;
        }

        yield return new WaitForSeconds(_waitSeconds);

        _state = new FlyState(gameObject, _flyHeight, _flySpeed, _hand);
        _stateEnum = HammerState.Fly;
        _hasTouchedGround = false;
    }

    private enum HammerState
    {
        Idle,
        Fly,
        Return
    }
}
