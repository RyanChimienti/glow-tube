using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IllegalDoubleHitEvent : UnityEvent<bool> {}

/// <summary>
/// Detects illegal double hits.
/// </summary>
public class DoubleHitDetector : MonoBehaviour {
    public IllegalDoubleHitEvent IllegalDoubleHitEvent = new IllegalDoubleHitEvent();

    private bool _playerHitLast;
    private DateTime _earliestHitTime;

    public void OnRoundStart() {
        _playerHitLast = false;
        _earliestHitTime = System.DateTime.Now;
    }

    public void OnBallHit(bool playerHit) {
        bool isDoubleHit = playerHit == _playerHitLast;
        if (isDoubleHit) {
            string nameOfHitter = playerHit ? "player" : "opponent";
            double timeSinceEarliestHit = System.DateTime.Now.Subtract(_earliestHitTime).TotalSeconds;
            if (timeSinceEarliestHit > GameConstants.DOUBLE_HIT_TOLERANCE) {
                if (GameConstants.DEBUG_MODE) {
                    Utils.DebugLog($"Illegal double hit by {nameOfHitter}.");
                }

                IllegalDoubleHitEvent.Invoke(playerHit);
            }
            else if (GameConstants.DEBUG_MODE) {
                Utils.DebugLog(
                    $" Technically a double hit by {nameOfHitter}, but it's close enough to the" +
                    $" first hit that we just consider it part of the first hit.");                
            }
        }
        else {
            _playerHitLast = playerHit;
            _earliestHitTime = System.DateTime.Now;
        }
    }
}
