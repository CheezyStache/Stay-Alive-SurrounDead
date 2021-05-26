using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnPlayerWaveChange", menuName = "On Player Wave Change Event", order = 4)]
public class OnPlayerWaveChangeEvent : GameEvent
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private OnBossWaveStart onBossWaveStart;
    [SerializeField] private int bossWave;

    public void NextWave()
    {
        playerData.Wave++;

        if (playerData.Wave % bossWave == bossWave - 1)
        {
            onBossWaveStart.Raise();
            return;
        }

        Raise();
    }

    public void ResetWaves()
    {
        playerData.Wave = 0;
        Raise();
    }
}
