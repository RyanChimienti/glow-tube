using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorekeeperManager : MonoBehaviour {
    public int PlayerScore { get; private set;  }
    public int OpponentScore { get; private set; }
    
    private void Awake() {
        PlayerScore = 0;
        OpponentScore = 0;
    }

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        if (playerWon) {
            PlayerScore += 1;
        }
        else {
            OpponentScore += 1;
        }
    }
}
