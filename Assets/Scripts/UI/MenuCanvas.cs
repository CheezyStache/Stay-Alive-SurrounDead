using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : GameEventListener
{
    public void OnWaveStart()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
}
