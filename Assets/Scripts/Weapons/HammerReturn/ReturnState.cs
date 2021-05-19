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
        private readonly float[] _parts;
        private readonly float _distance;

        private float _currentSecond;
        private int _partIndex;

        private const int ROTATION_TIMES = 3;

        public ReturnState(GameObject gameObject, float returnTime, GameObject hand)
        {
            _gameObject = gameObject;
            _hand = hand;

            _distance = Vector3.Distance(gameObject.transform.position, hand.transform.position);
            _parts = GetPartsPerSecond(returnTime);

            _direction = hand.transform.position - _gameObject.transform.position;
            _direction.Normalize();

            _currentSecond = 1f;
            _partIndex = 0;
        }

        public bool StateUpdate()
        {
            if (_partIndex >= _parts.Length)
                return false;

            var speed = _distance * _parts[_partIndex];
            var rotationSpeed = ROTATION_TIMES * 360f * _parts[_partIndex];
            var delta = Time.deltaTime;
            bool last = false;
            if (delta > _currentSecond)
            {
                delta = _currentSecond;
                _currentSecond = 1f;
                _partIndex++;
                last = true;
            }

            _gameObject.transform.position =
                Vector3.MoveTowards(_gameObject.transform.position, _hand.transform.position, speed * delta);
            _gameObject.transform.Rotate(Vector3.forward, rotationSpeed * delta);

            if (!last)
                _currentSecond -= delta;

            return true;
        }

        public bool CheckCollision(Collision collision)
        {
            return false;
        }

        private Vector3 GetRotationToHand()
        {
            return new Vector3(ROTATION_TIMES * 360f, 0f, 0f);
        }

        private float[] GetPartsPerSecond(float seconds)
        {
            var intSeconds = Mathf.CeilToInt(seconds);
            if (intSeconds <= 0)
                throw new UnityException("Hammer can't return in 0 seconds!");

            var parts = new float[intSeconds];
            parts[0] = 0.5f;

            for (int i = 1; i < parts.Length - 1; i++)
                parts[i] = parts[i - 1] / 2;

            if (parts.Length != 1)
                parts[parts.Length - 1] = parts[parts.Length - 2];

            return parts.Reverse().ToArray();
        }
    }
}