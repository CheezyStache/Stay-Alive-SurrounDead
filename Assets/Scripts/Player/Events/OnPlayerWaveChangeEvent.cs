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
        if ((playerData.Wave + 1) % bossWave == bossWave - 1)
        {
            onBossWaveStart.Raise();
            return;
        }

        playerData.Wave++;
        Raise();
    }

    public void BossWaveFinished()
    {
        playerData.Wave += 2;
        Raise();
    }

    public void ResetWaves()
    {
        onBossWaveStart.Raise();

        //playerData.Wave = 0;
        //Raise();
    }
}
