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
    /// <summary>
    /// The amount of time (in seconds) that must have passed between
    /// two hits for them to be considered an illegal double hit.
    /// </summary>
    private static double DOUBLE_HIT_TOLERANCE = 0.5;

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
        ToggleControllersActive();

        playMenu.SetActive(false);
        ball.SetActive(true);
        ball.GetComponent<Rigidbody>().position = playMenu.transform.position;
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -20);
        
        GameState.CurrentStatus = GameState.Status.PLAYING_ROUND;
        GameState.PlayerHitLast = false;
        GameState.MostRecentTurnChange = System.DateTime.Now;
    }

    public void EndRound(bool playerWon) {
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

    public void HandleBallHit(bool playerHit) {
        // This is a double hit, so check if it's illegal.
        if (playerHit == GameState.PlayerHitLast) {
            if (System.DateTime.Now.Subtract(GameState.MostRecentTurnChange).TotalSeconds 
                > DOUBLE_HIT_TOLERANCE) {
                EndRound(!playerHit);
            }
        }
        // This is a first hit, so update the turn info.
        else {
            GameState.PlayerHitLast = playerHit;
            GameState.MostRecentTurnChange = System.DateTime.Now;
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
