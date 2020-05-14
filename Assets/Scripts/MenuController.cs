using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public GameObject scoreText;

    void Start() {
        scoreText.GetComponent<Text>().text = $"Ready?";
    }

    private void OnEnable() {
        int playerScore = GameController.PlayerScore;
        int oppScore = GameController.OpponentScore;
        if (playerScore > oppScore) {
            scoreText.GetComponent<Text>().text = $"Winning {playerScore} - {oppScore}";
        }
        else if (playerScore < oppScore) {
            scoreText.GetComponent<Text>().text = $"Losing {playerScore} - {oppScore}";
        }
        else {
            scoreText.GetComponent<Text>().text = $"Tied {playerScore} - {oppScore}";
        }
    }
}
