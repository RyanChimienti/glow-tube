using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides methods for high-level game actions.
/// </summary>
public class GameController : MonoBehaviour {
    public GameObject ball;
    public GameObject menu;

    public void Start() {
        GameState.CurrentStatus = GameState.Status.IN_MENU;
        GameState.PlayerScore = 0;
        GameState.OpponentScore = 0;

        ball.SetActive(false);
        menu.SetActive(true);
    }

    public void StartNewRound() {
        menu.SetActive(false);
        ball.SetActive(true);
        ball.GetComponent<Rigidbody>().position = menu.transform.position;
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -20);
        GameState.CurrentStatus = GameState.Status.PLAYING_ROUND;
    }

    public void EndRound(bool playerWon) {
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
}
