using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenuManager : MonoBehaviour {
    /// <summary>
    /// The Text component in the UI where the score should be displayed.
    /// </summary>
    public Text ScoreText;
    
    /// <summary>
    /// The scorekeeper script.
    /// </summary>
    public ScorekeeperManager Scorekeeper;

    private void OnEnable() {
        int playerScore = Scorekeeper.PlayerScore;
        int oppScore = Scorekeeper.OpponentScore;

        if (playerScore > oppScore) {
            ScoreText.text = $"Winning {playerScore} - {oppScore}";
        }
        else if (playerScore < oppScore) {
            ScoreText.text = $"Losing {playerScore} - {oppScore}";
        }
        else if (playerScore == 0) {
            ScoreText.text = $"Ready?";
        }
        else {
            ScoreText.text = $"Tied {playerScore} - {oppScore}";
        }
    }
}
