using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static int PlayerScore;
    public static int OpponentScore;

    public GameObject ball;
    public GameObject menu;

    void Start() {
        GameController.PlayerScore = 0;
        GameController.OpponentScore = 0;

        ball.SetActive(false);
        menu.SetActive(true);
    }

    public void StartNewRound() {
        menu.SetActive(false);
        ball.SetActive(true);
        ball.GetComponent<Rigidbody>().position = menu.transform.position;
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -20);
    }

    public void EndRound(bool playerWon) {
        if (playerWon) {
            PlayerScore++;
        }
        else {
            OpponentScore++;
        }

        menu.SetActive(true);
        ball.SetActive(false);
    }
}
