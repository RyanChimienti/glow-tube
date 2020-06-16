using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine.Events;
using System.Diagnostics.Tracing;

[System.Serializable]
public class RoundEndEvent : UnityEvent<bool, OutcomeReason> {}

/// <summary>
/// Provides methods for high-level game actions.
/// </summary>
public class RoundManager : MonoBehaviour { 
    [Header("Triggers when a new round starts.")]
    public UnityEvent RoundStartEvent = new UnityEvent();

    [Header("Triggers when a round ends (the moment the outcome is known).")]
    public RoundEndEvent RoundEndEvent = new RoundEndEvent();

    public void OnPlayRoundButtonPress() {
        StartNewRound();
    }

    private void StartNewRound() {
        if (GameConstants.DEBUG_MODE) {
            Utils.DebugLog($"Round started.");
        }

        RoundStartEvent.Invoke();
    }

    public void OnBounceLimitExceeded(bool isForPlayer) {
        EndRound(!isForPlayer, OutcomeReason.BOUNCE_LOSS);
    }

    public void OnDoubleHit(bool isForPlayer) {
        EndRound(!isForPlayer, OutcomeReason.DOUBLE_HIT);
    }

    public void OnLetThrough(bool isForPlayer) {
        EndRound(!isForPlayer, OutcomeReason.LET_THROUGH);
    }

    private void EndRound(bool playerWon, OutcomeReason reason) {
        if (GameConstants.DEBUG_MODE) {
            string name = playerWon ? "player" : "opponent";
            string reasonStr = reason.ToReadableString();
            Utils.DebugLog($"Round ended: {name} wins because of {reasonStr}.");
        }

        RoundEndEvent.Invoke(playerWon, reason);
    }  
}
