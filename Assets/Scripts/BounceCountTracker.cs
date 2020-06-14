using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BounceLimitExceededEvent : UnityEvent<bool> {}

/// <summary>
/// Keeps track of the number of times the ball has hit the tunnel walls
/// in the current turn, gradually changing the color of the ball and 
/// triggering an event when the bounce limit is exceeded.
/// </summary>
public class BounceCountTracker : MonoBehaviour {
    /// <summary>
    /// Triggers when the maximum number of bounces on a turn is
    /// exceeded.
    /// </summary>
    public BounceLimitExceededEvent BounceLimitExceededEvent = new BounceLimitExceededEvent();

    /// <summary>
    /// Whether we are currently counting the player's bounces (that is,
    /// if it's currently the player's turn.)
    /// </summary>
    private bool _trackingBouncesForPlayer = false;

    /// <summary>
    /// The number of times the ball has hit one of the walls this turn.
    /// </summary>
    private int _numWallBouncesThisTurn;

    /// <summary>
    /// The point light within the ball.
    /// </summary>
    private Light _ballLight;

    /// <summary>
    /// The material for the ball.
    /// </summary>
    private Material _ballMaterial;

    private void Awake() {
        _ballLight = this.GetComponent<Light>();
        _ballMaterial = this.GetComponent<MeshRenderer>().material;
    }

    public void OnRoundStart() {
        _numWallBouncesThisTurn = 0;
        setBallColor(GameConstants.BALL_START_COLOR);
    }

    private void OnCollisionEnter(Collision collision) {
        Transform colliderObjParent = collision.collider.gameObject.transform.parent;

        if (colliderObjParent != null && colliderObjParent.gameObject.tag == "Tunnel") {
            _numWallBouncesThisTurn++;

            if (_numWallBouncesThisTurn == GameConstants.NUM_BOUNCES_FOR_LOSS) {
                BounceLimitExceededEvent.Invoke(_trackingBouncesForPlayer);
            }
            else {
                float percentageOfBounceLimit = (float)_numWallBouncesThisTurn / GameConstants.NUM_BOUNCES_FOR_LOSS;
                setBallColor(Color.Lerp(
                    GameConstants.BALL_START_COLOR,
                    GameConstants.BALL_SHATTER_COLORS[OutcomeReason.BOUNCE_LOSS],
                    percentageOfBounceLimit));
            }            
        }        
    }

    public void OnTurnChange(bool isPlayersTurn) {
        _trackingBouncesForPlayer = isPlayersTurn;
        _numWallBouncesThisTurn = 0;
        setBallColor(GameConstants.BALL_START_COLOR);
    }

    private void setBallColor(Color color) {
        _ballLight.color = color;
        _ballMaterial.SetColor("_EmissionColor", color);
        _ballMaterial.SetColor("_SpecColor", color);
    }
}
