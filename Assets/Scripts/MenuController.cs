using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public GameObject ScoreText;

    void Start() {
        ScoreText.GetComponent<Text>().text = $"Ready?";
    }

    private void OnEnable() {
        int playerScore = GameState.PlayerScore;
        int oppScore = GameState.OpponentScore;
        if (playerScore > oppScore) {
            ScoreText.GetComponent<Text>().text = $"Winning {playerScore} - {oppScore}";
        }
        else if (playerScore < oppScore) {
            ScoreText.GetComponent<Text>().text = $"Losing {playerScore} - {oppScore}";
        }
        else {
            ScoreText.GetComponent<Text>().text = $"Tied {playerScore} - {oppScore}";
        }
    }
}
