using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnPlayerHealthChange", menuName = "On Player Health Change Event", order = 3)]
public class OnPlayerHealthChangeEvent : GameEvent
{
    public void Raise(int change)
    {
        if (change != 0)
            Raise();
    }
}