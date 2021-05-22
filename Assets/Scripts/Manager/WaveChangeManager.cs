using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveChangeManager : GameEventListener
{
    [SerializeField] private OnPlayerWaveChangeEvent onPlayerWaveChangeEvent;
    [SerializeField] private Transform enemies;
    [SerializeField] private float changeDelay;

    private WaveEndedCheck waveEndedCheck;

    void Start()
    {
        waveEndedCheck = GetComponent<WaveEndedCheck>();
    }

    public void CheckWaveUpdate()
    {
        StartCoroutine(CheckWave());
    }

    IEnumerator CheckWave()
    {
        yield return new WaitForSeconds(changeDelay);

        if (waveEndedCheck.IsWaveEnded && enemies.childCount == 0)
            ChangeWave();
    }

    private void ChangeWave()
    {
        waveEndedCheck.ResetValue();
        onPlayerWaveChangeEvent.NextWave();
    }
}
