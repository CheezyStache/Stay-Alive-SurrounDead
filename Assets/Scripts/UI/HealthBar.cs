using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : GameEventListener
{
    [SerializeField] private PlayerData playerData;

    private Text textObj;

    void Start()
    {
        textObj = GetComponent<Text>();
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        var health = playerData.Health < 0 ? 0 : playerData.Health;
        textObj.text = health.ToString();
    }
}
