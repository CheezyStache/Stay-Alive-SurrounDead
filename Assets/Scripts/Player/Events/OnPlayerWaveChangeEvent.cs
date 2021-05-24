using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnPlayerWaveChange", menuName = "On Player Wave Change Event", order = 4)]
public class OnPlayerWaveChangeEvent : GameEvent
{
    [SerializeField] private PlayerData playerData;

    public void NextWave()
    {
        playerData.Wave++;
        Raise();
    }

    public void ResetWaves()
    {
        playerData.Wave = 0;
        Raise();
    }
}
