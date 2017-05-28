using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunVision : MonoBehaviour {

    void OnTriggerEnter(Collider coll)
    {
        PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
        if (pc && !pc.isSeeker)
        {
            pc.CmdFreeze(true);
        }
    }
    void OnTriggerExit(Collider coll)
    {
        PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
        if (pc)
        {
            pc.CmdFreeze(false);
        }
    }
}
