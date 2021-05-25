using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BossStageHands : GameEventListener
{
    [SerializeField] private GameObject sceneHammer;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject shield;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    private bool isBoss;
    private GameObject shieldInstance;
    private GameObject hammerInstance;

    public void ConnectToHands()
    {
        isBoss = true;

        sceneHammer.SetActive(false);

        shieldInstance = Instantiate(shield, Vector3.zero, Quaternion.identity);
        hammerInstance = Instantiate(hammer, Vector3.zero, Quaternion.identity);

        leftHand.GetComponent<Hand>().AttachObject(shieldInstance, GrabTypes.Grip);
        rightHand.GetComponent<Hand>().AttachObject(hammerInstance, GrabTypes.Grip);
    }

    public void ReleaseHands()
    {
        if (!isBoss)
            return;

        sceneHammer.SetActive(true);

        leftHand.GetComponent<Hand>().DetachObject(shieldInstance);
        rightHand.GetComponent<Hand>().DetachObject(hammerInstance);

        Destroy(shieldInstance);
        Destroy(hammerInstance);
    }
}
