using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BossStageHands : GameEventListener
{
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject shield;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    private bool isBoss;
    private GameObject shieldInstance;

    public void ConnectToHands()
    {
        isBoss = true;

        shieldInstance = Instantiate(shield, Vector3.zero, Quaternion.identity);
        hammer.GetComponent<Rigidbody>().isKinematic = true;
        hammer.GetComponent<HammerReturn>().enabled = false;

        leftHand.GetComponent<Hand>().AttachObject(shieldInstance, GrabTypes.Grip);
        rightHand.GetComponent<Hand>().AttachObject(hammer, GrabTypes.Grip);
    }

    public void ReleaseHands()
    {
        if (!isBoss)
            return;

        leftHand.GetComponent<Hand>().DetachObject(shieldInstance);
        rightHand.GetComponent<Hand>().DetachObject(hammer);

        Destroy(shieldInstance);
        hammer.GetComponent<Rigidbody>().isKinematic = false;
        hammer.GetComponent<HammerReturn>().enabled = true;
    }
}
