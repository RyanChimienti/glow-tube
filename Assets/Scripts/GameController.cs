using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using System.Runtime.ConstrainedExecution;

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

    /// <summary>
    /// True if the player is holding controllers; false if
    /// the player is holding the paddle.
    /// </summary>
    private bool controllersActive = true;    

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
        controllersActive = !controllersActive;

        // Before activating the paddle, make sure it's in the correct hand.
        if (PlayerPrefs.HasKey("PaddleInLeftHand")) {
            paddle.GetComponent<PaddleController>().LeftHand = Convert.ToBoolean(
                PlayerPrefs.GetInt("PaddleInLeftHand")
            );
        }
        paddle.SetActive(!controllersActive);

        leftHand.GetComponent<XRController>().hideControllerModel = !controllersActive;
        leftHand.GetComponent<XRRayInteractor>().enabled = controllersActive;
        leftControllerUI.SetActive(controllersActive);

        rightHand.GetComponent<XRController>().hideControllerModel = !controllersActive;
        rightHand.GetComponent<XRRayInteractor>().enabled = controllersActive;
        rightControllerUI.SetActive(controllersActive);
    }

    private void EnableBallCollider() {
        ball.GetComponent<SphereCollider>().enabled = true;
    }

    public void EndRound(bool playerWon) {
        if (GameConstants.DEBUG_MODE) {
            string name = playerWon ? "player" : "opponent";
            Utils.DebugLog($"Round ended: {name} wins.");
        }

        ToggleControllersActive();

        if (playerWon) {
            GameState.PlayerScore++;
        }
        else {
            GameState.OpponentScore++;
        }

        playMenu.SetActive(true);
        ball.SetActive(false);
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

            if (GameConstants.DEBUG_MODE) {
                Utils.DebugLog($"Ball hit by {(playerHit ? "player" : "opponent")}.");
            }
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
