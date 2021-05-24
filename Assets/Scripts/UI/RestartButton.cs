using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private OnPlayerWaveChangeEvent onPlayerWaveChangeEvent;
    [SerializeField] private PlayerData playerData;

    public void Restart()
    {
        playerData.RestartHealth();
        onPlayerWaveChangeEvent.ResetWaves();
    }
}
