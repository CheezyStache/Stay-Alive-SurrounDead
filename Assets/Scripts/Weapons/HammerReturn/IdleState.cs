using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HammerState
{
    public class IdleState: IHammerState
    {
        private GameObject _gameObject;
        private Vector3 _prevPos;

        private const float ACCURACY = 0.001f;

        public IdleState(GameObject gameObject)
        {
            _gameObject = gameObject;
            _prevPos = gameObject.transform.position;

            _gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        public bool StateUpdate()
        {
            return true;
        }

        public bool CheckCollision(Collision collision)
        {
            if (IsMoving())
                return false;

            return collision.gameObject.layer == LayerMask.NameToLayer("Ground");
        }

        private bool IsMoving()
        {
            var distance = Vector3.Distance(_prevPos, _gameObject.transform.position);
            _prevPos = _gameObject.transform.position;
            return distance > ACCURACY;
        }
    }
}