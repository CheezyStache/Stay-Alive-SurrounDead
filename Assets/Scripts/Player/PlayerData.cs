using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int maxHealth;

    public int Health { get; set; }
    public int Wave { get; set; }

    void OnEnable()
    {
        Health = maxHealth;
        Wave = 0;
    }
}
