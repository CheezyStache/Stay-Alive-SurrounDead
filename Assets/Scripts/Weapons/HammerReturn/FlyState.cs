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

        private float _flySpeed;

        private const float DISTANCE_ACCURACY = 0.05f;
        private const float ROTATION_ACCURACY = 5f;
        private const float HAMMER_ROTATION_OFFSET = -90f;

        public FlyState(GameObject gameObject, float flyHeight, float flyTime, GameObject hand)
        {
            _gameObject = gameObject;
            _flyToPosition = gameObject.transform.position + new Vector3(0f, flyHeight, 0f);
            _flyToRotation = GetRotationToHand(hand, _flyToPosition);
            _flySpeed = Vector3.Distance(gameObject.transform.position, _flyToPosition) / flyTime;

            _gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        public bool StateUpdate()
        {
            var smallDistance = IsPreciseDistance();
            var smallRotation = IsPreciseRotation();

            if (smallDistance && smallRotation)
                return false;

            var direction = _flyToPosition - _gameObject.transform.position;
            direction.Normalize();


            if (!smallDistance)
                _gameObject.transform.position = Vector3.MoveTowards(_gameObject.transform.position, _flyToPosition,
                    _flySpeed * Time.deltaTime);

            if(!smallRotation)
                _gameObject.transform.rotation = Quaternion.Lerp(_gameObject.transform.rotation, _flyToRotation, Time.deltaTime);

            return true;
        }

        public bool CheckCollision(Collision collision)
        {
            return false;
        }

        private bool IsPreciseDistance()
        {
            return Vector3.Distance(_gameObject.transform.position, _flyToPosition) <= DISTANCE_ACCURACY;
        }

        private bool IsPreciseRotation()
        {
            var degrees = _gameObject.transform.eulerAngles - _flyToRotation.eulerAngles;
            return IsPreciseEulerAngle(degrees.x) && IsPreciseEulerAngle(degrees.y) &&
                   IsPreciseEulerAngle(degrees.z);
        }

        private bool IsPreciseEulerAngle(float angle)
        {
            return Mathf.Abs(angle) <= ROTATION_ACCURACY || 360f - Mathf.Abs(angle) <= ROTATION_ACCURACY;
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