using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LetThroughEvent : UnityEvent<bool> {}

public class DetectLetThrough : MonoBehaviour {
    [Header("The argument is true if the let through was on the player's side.")]
    [SerializeField]
    public LetThroughEvent LetThroughEvent = new LetThroughEvent();

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
            LetThroughEvent.Invoke(IsForPlayer);
        }    
    }
}
