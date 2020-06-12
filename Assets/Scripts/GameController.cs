using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine.Events;
using System.Diagnostics.Tracing;

/// <summary>
/// Provides methods for high-level game actions.
/// </summary>
public class GameController : MonoBehaviour { 

    public GameObject ball;
    public GameObject playMenu;
    public GameObject settingsMenu;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftControllerUI;
    public GameObject rightControllerUI;
    public GameObject paddle;

    [Tooltip("The prefab to use for the shattered ball.")]
    public GameObject ShatteredBallPrefab;

    [Tooltip("The GameObject containing (in its children) the ball shatter sound and particles.")]
    public GameObject BallShatterEffects;

    [Header("Event that triggers when the ball changes possession")]
    public UnityEvent TurnChangeEvent = new UnityEvent();

    /// <summary>
    /// True if the player is holding controllers; false if
    /// the player is holding the paddle.
    /// </summary>
    private bool _controllersActive = true;
    
    /// <summary>
    /// The shattered ball game object, which is created at the end
    /// of a round and destroyed when returning to the menu.
    /// </summary>
    private GameObject _shatteredBall;

    public void Start() {
        GameState.CurrentStatus = GameState.Status.IN_MENU;
        GameState.PlayerScore = 0;
        GameState.OpponentScore = 0;

        paddle.SetActive(false);
        ball.SetActive(false);
        playMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void StartNewRound() {
        if (GameConstants.DEBUG_MODE) {
            Utils.DebugLog($"Round started.");
        }

        ToggleControllersActive();

        playMenu.SetActive(false);

        ball.transform.position = playMenu.transform.position;
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1.5f);
        ball.SetActive(true);

        // Disable the ball's collider for 3 physics steps to prevent a strange collision that
        // sometimes occurs between the ball and the paddle right after starting a round.
        // (Specifically, this collision was happening after some double hit losses.)
        ball.GetComponent<SphereCollider>().enabled = false;
        Invoke("EnableBallCollider", Time.fixedDeltaTime * 3);

        GameState.CurrentStatus = GameState.Status.PLAYING_ROUND;
        GameState.PlayerHitLast = false;
        GameState.MostRecentTurnChange = System.DateTime.Now;
        GameState.TurnNumber = 0;
    }

    private void ToggleControllersActive() {
        _controllersActive = !_controllersActive;

        // Before activating the paddle, make sure it's in the correct hand.
        if (PlayerPrefs.HasKey("PaddleInLeftHand")) {
            paddle.GetComponent<PaddleController>().LeftHand = Convert.ToBoolean(
                PlayerPrefs.GetInt("PaddleInLeftHand")
            );
        }
        paddle.SetActive(!_controllersActive);

        leftHand.GetComponent<XRController>().hideControllerModel = !_controllersActive;
        leftHand.GetComponent<XRRayInteractor>().enabled = _controllersActive;
        leftControllerUI.SetActive(_controllersActive);

        rightHand.GetComponent<XRController>().hideControllerModel = !_controllersActive;
        rightHand.GetComponent<XRRayInteractor>().enabled = _controllersActive;
        rightControllerUI.SetActive(_controllersActive);
    }

    private void EnableBallCollider() {
        ball.GetComponent<SphereCollider>().enabled = true;
    }

    public void EndRound(bool playerWon) {
        if (GameConstants.DEBUG_MODE) {
            string name = playerWon ? "player" : "opponent";
            Utils.DebugLog($"Round ended: {name} wins.");
        }        

        if (playerWon) {
            GameState.PlayerScore++;
        }
        else {
            GameState.OpponentScore++;
        }

        ShatterBall();
        GameState.CurrentStatus = GameState.Status.ROUND_JUST_ENDED;
        Invoke("ReturnToMenuAfterRound", GameConstants.RETURN_TO_MENU_DELAY);
    }    

    private void ShatterBall() {
        BallShatterEffects.transform.position = ball.transform.position;
        BallShatterEffects.GetComponentInChildren<AudioSource>().Play();
        BallShatterEffects.GetComponentInChildren<ParticleSystem>().Play();

        _shatteredBall = Instantiate(
            ShatteredBallPrefab,
            ball.transform.position,
            Quaternion.identity
        );        

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
        ToggleControllersActive();
        playMenu.SetActive(true);        
        GameState.CurrentStatus = GameState.Status.IN_MENU;
    }

    public void HandleBallHit(bool playerHit) {
        if (playerHit == GameState.PlayerHitLast) { // Double hit, check if allowable
            if (System.DateTime.Now.Subtract(GameState.MostRecentTurnChange).TotalSeconds 
                > GameConstants.DOUBLE_HIT_TOLERANCE) {

                if (GameConstants.DEBUG_MODE) {
                    Utils.DebugLog($"Ball hit by {(playerHit ? "player" : "opponent")}." +
                        $" Illegal double hit!");
                }

                EndRound(!playerHit);
            }
            else { 
                if (GameConstants.DEBUG_MODE) {
                    Utils.DebugLog($"Ball hit by {(playerHit ? "player" : "opponent")}." +
                        $" Technically a double hit, but it's close enough to the" +
                        $" first hit that we just consider it part of the first hit.");
                }
            }
        }
        
        else { // First hit, update the turn info
            GameState.PlayerHitLast = playerHit;
            GameState.MostRecentTurnChange = System.DateTime.Now;
            GameState.TurnNumber++;
            GameState.NumWallBouncesThisTurn = 0;
            
            if (GameConstants.DEBUG_MODE) {
                Utils.DebugLog($"Ball hit by {(playerHit ? "player" : "opponent")}.");
            }

            TurnChangeEvent.Invoke();
        }        
    }

    public void OpenSettingsMenu() {
        playMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenPlayMenu() {
        playMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
