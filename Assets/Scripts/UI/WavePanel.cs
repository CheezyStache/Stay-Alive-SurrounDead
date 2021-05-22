using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavePanel : GameEventListener
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Text waveText;
    [SerializeField] private Text waveLabel;
    [SerializeField] private float showTime;

    void Start()
    {
        UpdateWave();
    }

    public void UpdateWave()
    {
        waveText.text = (playerData.Wave + 1).ToString();
        StartCoroutine(ShowWaveTimer());
    }

    private IEnumerator ShowWaveTimer()
    {
        waveText.enabled = true;
        waveLabel.enabled = true;

        yield return new WaitForSeconds(showTime);

        waveText.enabled = false;
        waveLabel.enabled = false;
    }
}
