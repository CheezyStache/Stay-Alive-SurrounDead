using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace HammerState
{
    public class FlyState : IHammerState
    {
        private GameObject _gameObject;

        private Vector3 _flyToPosition;
        private Quaternion _flyToRotation;

        private readonly float _flyTime;
        private readonly Vector3 _startPosition;
        private readonly Quaternion _startRotation;

        private float _leftTime;

        private const float HAMMER_ROTATION_OFFSET = -90f;

        public FlyState(GameObject gameObject, float flyHeight, float flyTime, GameObject hand)
        {
            _gameObject = gameObject;
            _flyToPosition = gameObject.transform.position + new Vector3(0f, flyHeight, 0f);
            _flyToRotation = GetRotationToHand(hand, _flyToPosition);

            _gameObject.GetComponent<Rigidbody>().isKinematic = true;

            _leftTime = flyTime;
            _flyTime = flyTime;
            _startPosition = gameObject.transform.position;
            _startRotation = gameObject.transform.rotation;
        }

        public bool StateUpdate()
        {
            if (_leftTime <= 0f)
                return false;

            _leftTime -= Time.deltaTime;

            var interpolation = (_flyTime - _leftTime) / _flyTime;

            _gameObject.transform.position = Vector3.Lerp(_startPosition, _flyToPosition, interpolation);
            _gameObject.transform.rotation = Quaternion.Lerp(_startRotation, _flyToRotation, interpolation);

            return true;
        }

        public bool CheckCollision(Collision collision)
        {
            return false;
        }

        private Quaternion GetRotationToHand(GameObject hand, Vector3 position)
        {
            var direction = hand.transform.position - position;
            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            var rotation = Quaternion.Euler(lookRotation.eulerAngles.x,
                lookRotation.eulerAngles.y - HAMMER_ROTATION_OFFSET, lookRotation.eulerAngles.z);

            return rotation;
        }
    }
}