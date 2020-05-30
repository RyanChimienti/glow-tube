using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods for high-level game actions.
/// </summary>
public class GameController : MonoBehaviour {
    public GameObject ball;
    public GameObject menu;
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
        menu.SetActive(true);
    }

    public void StartNewRound() {
        ToggleControllersActive();

        menu.SetActive(false);
        ball.SetActive(true);
        ball.GetComponent<Rigidbody>().position = menu.transform.position;
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -20);
        GameState.CurrentStatus = GameState.Status.PLAYING_ROUND;
    }

    public void EndRound(bool playerWon) {
        ToggleControllersActive();

        if (playerWon) {
            GameState.PlayerScore++;
        }
        else {
            GameState.OpponentScore++;
        }

        menu.SetActive(true);
        ball.SetActive(false);
        GameState.CurrentStatus = GameState.Status.IN_MENU;
    }

    private void ToggleControllersActive() {
        controllersActive = !controllersActive;
        paddle.SetActive(!controllersActive);

        leftHand.GetComponent<XRController>().hideControllerModel = !controllersActive;
        leftHand.GetComponent<XRRayInteractor>().enabled = controllersActive;
        leftControllerUI.SetActive(controllersActive);

        rightHand.GetComponent<XRController>().hideControllerModel = !controllersActive;
        rightHand.GetComponent<XRRayInteractor>().enabled = controllersActive;
        rightControllerUI.SetActive(controllersActive);        
    }
}
