using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HammerState
{
    public class HoldingState : IHammerState
    {
        public HoldingState(GameObject gameObject)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        public bool StateUpdate()
        {
            return true;
        }

        public bool CheckCollision(Collision collision)
        {
            return false;
        }
    }
}