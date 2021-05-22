using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndedCheck : GameEventListener
{
    public bool IsWaveEnded { get; private set; }

    public void SetWaveEnded()
    {
        IsWaveEnded = true;
    }

    public void ResetValue()
    {
        IsWaveEnded = false;
    }
}
