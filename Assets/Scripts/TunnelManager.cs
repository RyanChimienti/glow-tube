using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the changing colors of the tunnel walls.
/// </summary>
public class TunnelManager : MonoBehaviour {
    private void Start() {
        SetTunnelColor(GameConstants.TUNNEL_START_COLOR);
    }

    public void OnTurnChange(bool isPlayersTurn) {
        Color newTunnelColor = isPlayersTurn ?
                GameConstants.PLAYER_TURN_TUNNEL_COLOR :
                GameConstants.OPPONENT_TURN_TUNNEL_COLOR;
        SetTunnelColor(newTunnelColor);        
    }

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        SetTunnelColor(GameConstants.TUNNEL_START_COLOR);
    }

    private void SetTunnelColor(Color color) {
        foreach (MeshRenderer wallRenderer in this.GetComponentsInChildren<MeshRenderer>()) {
            wallRenderer.material.SetColor("_Color", color);
        }
    }
}
