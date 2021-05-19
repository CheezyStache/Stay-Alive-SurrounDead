using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HammerState
{
    public interface IHammerState
    {
        bool StateUpdate();

        bool CheckCollision(Collision collision);
    }
}