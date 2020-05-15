using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLetThrough : MonoBehaviour {
    public GameObject GameControllerObj;

    /**
     * True if detecting the player's let through; false if detecting
     * the opponent's.
     */ 
    public bool IsForPlayer;

    void Start() {
        // Hide this since it's just a trigger.
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.gameObject.tag == "Ball") {
            GameControllerObj.GetComponent<GameController>().EndRound(!IsForPlayer);
        }    
    }
}
