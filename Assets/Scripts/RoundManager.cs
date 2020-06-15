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
    public GameObject ball;

    [Header("Triggers when a new round starts.")]
    public UnityEvent RoundStartEvent = new UnityEvent();

    [Header("Triggers when a round ends (the moment the outcome is known).")]
    public RoundEndEvent RoundEndEvent = new RoundEndEvent();

    public void Start() {        
        ball.SetActive(false);
    }

    public void OnPlayRoundButtonPress() {
        StartNewRound();
    }

    private void StartNewRound() {
        if (GameConstants.DEBUG_MODE) {
            Utils.DebugLog($"Round started.");
        }

        ball.transform.position = new Vector3(0f, .9f, 1f);
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1.5f);
        ball.SetActive(true);

        // Disable the ball's collider for 3 physics steps to prevent a strange collision that
        // sometimes occurs between the ball and the paddle right after starting a round.
        // (Specifically, this collision was happening after some double hit losses.)
        ball.GetComponent<SphereCollider>().enabled = false;
        Invoke("EnableBallCollider", Time.fixedDeltaTime * 3);

        RoundStartEvent.Invoke();
    }    

    private void EnableBallCollider() {
        ball.GetComponent<SphereCollider>().enabled = true;
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
