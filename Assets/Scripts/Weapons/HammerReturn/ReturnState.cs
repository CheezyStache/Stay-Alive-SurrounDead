using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HammerState
{
    public class ReturnState : IHammerState
    {
        private GameObject _gameObject;
        
        private GameObject _hand;

        private readonly Vector3 _direction;
        private readonly float _distance;
        private readonly float _acceleration;
        private readonly float _rotationAcceleration;

        private float _speed;
        private float _rotationSpeed;
        private float _leftTime;

        public ReturnState(GameObject gameObject, float returnTime, int rotationTimes, GameObject hand)
        {
            _gameObject = gameObject;
            _hand = hand;

            _distance = Vector3.Distance(gameObject.transform.position, hand.transform.position);

            _direction = hand.transform.position - _gameObject.transform.position;
            _direction.Normalize();

            _acceleration = 2 * _distance / returnTime / returnTime;
            _rotationAcceleration = 2 * 360f * rotationTimes / returnTime / returnTime;
            _speed = 0f;
            _rotationSpeed = 0f;
            _leftTime = returnTime;
        }

        public bool StateUpdate()
        {
            if (_leftTime <= 0f)
                return false;

            _speed += _acceleration * Time.deltaTime;
            _rotationSpeed += _rotationAcceleration * Time.deltaTime;
            _leftTime -= Time.deltaTime;

            _gameObject.transform.position =
                Vector3.MoveTowards(_gameObject.transform.position, _hand.transform.position, _speed * Time.deltaTime);
            _gameObject.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);

            return true;
        }

        public bool CheckCollision(Collision collision)
        {
            return false;
        }
    }
}