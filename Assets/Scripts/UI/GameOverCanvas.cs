using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvas : GameEventListener
{
    public void OnPlayerDie()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }
}
