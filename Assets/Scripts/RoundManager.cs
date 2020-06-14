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

    [Tooltip("The prefab to use for the shattered ball.")]
    public GameObject ShatteredBallPrefab;

    [Tooltip("The GameObject containing (in its children) the ball shatter sound and particles.")]
    public GameObject BallShatterEffects;

    [Header("Triggers when a new round starts.")]
    public UnityEvent RoundStartEvent = new UnityEvent();

    [Header("Triggers when a round ends (the moment the outcome is known).")]
    public RoundEndEvent RoundEndEvent = new RoundEndEvent();

    /// <summary>
    /// The shattered ball game object, which is created at the end
    /// of a round and destroyed when returning to the menu.
    /// </summary>
    private GameObject _shatteredBall;

    public void Start() {
        GameState.CurrentStatus = GameState.Status.IN_MENU;
        GameState.PlayerScore = 0;
        GameState.OpponentScore = 0;
        
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

        GameState.CurrentStatus = GameState.Status.PLAYING_ROUND;

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

        if (playerWon) {
            GameState.PlayerScore++;
        }
        else {
            GameState.OpponentScore++;
        }

        ShatterBall(GameConstants.BALL_SHATTER_COLORS[reason]);
        GameState.CurrentStatus = GameState.Status.ROUND_JUST_ENDED;
        Invoke("ReturnToMenuAfterRound", GameConstants.RETURN_TO_MENU_DELAY);
    }    

    private void ShatterBall(Color shatterColor) {
        BallShatterEffects.transform.position = ball.transform.position;
        BallShatterEffects.GetComponentInChildren<AudioSource>().Play();

        ParticleSystem particleSys = BallShatterEffects.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mainMod = particleSys.main;
        ParticleSystem.TrailModule trailMod = particleSys.trails;
        mainMod.startColor = shatterColor;
        trailMod.colorOverTrail = shatterColor;
        particleSys.Play();

        _shatteredBall = Instantiate(
            ShatteredBallPrefab,
            ball.transform.position,
            Quaternion.identity
        );

        foreach (MeshRenderer renderer in _shatteredBall.GetComponentsInChildren<MeshRenderer>()) {
            renderer.material.SetColor("_Color", shatterColor);
        }

        Vector3 ballVelocity = ball.GetComponent<Rigidbody>().velocity;
        foreach (Rigidbody r in _shatteredBall.GetComponentsInChildren<Rigidbody>()) {
            r.velocity = ballVelocity;
            r.AddExplosionForce(2.0f, _shatteredBall.transform.position, 0.2f, 0, ForceMode.Impulse);
        }
        ball.SetActive(false);        
    }

    private void ReturnToMenuAfterRound() {
        Destroy(_shatteredBall);
        _shatteredBall = null;       
        GameState.CurrentStatus = GameState.Status.IN_MENU;
    }
}
