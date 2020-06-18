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

    /// <summary>
    /// Has this detector already found a let through this round? We need this
    /// to prevent double detections.
    /// </summary>
    private bool _detectedAlreadyThisRound;

    public void OnRoundStart() {
        _detectedAlreadyThisRound = false;
    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.gameObject.tag == "Ball" && !_detectedAlreadyThisRound) {
            _detectedAlreadyThisRound = true;
            LetThroughEvent.Invoke(IsForPlayer);            
        }    
    }
}
