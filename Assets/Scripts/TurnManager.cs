using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Argument is true if it's now the player's turn, false if it's the
/// opponent's turn.
/// </summary>
[System.Serializable]
public class TurnChangeEvent : UnityEvent<bool> {};

public class TurnManager : MonoBehaviour { 
    [Header("Invoked when the turn changes (but not when a round starts)")]  
    public TurnChangeEvent TurnChangeEvent = new TurnChangeEvent();

    private bool _isPlayersTurn;

    public void OnRoundStart() {
        // A round start is not a turn *change*, so don't invoke the event
        _isPlayersTurn = false;
    }

    public void OnBallHit(bool playerHit) {
        if (playerHit != _isPlayersTurn) {
            _isPlayersTurn = playerHit;
            TurnChangeEvent.Invoke(playerHit);
        }
    }
}
