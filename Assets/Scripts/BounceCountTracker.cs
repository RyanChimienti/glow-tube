using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the number of times the ball has hit the tunnel walls
/// in a turn, gradually changing the color of the ball and triggering
/// a loss if the limit is exceeded.
/// </summary>
public class BounceCountTracker : MonoBehaviour {
    [Tooltip("The object holding the GameController script.")]
    public GameObject GameControllerObj;

    private GameController _gameController;

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

    private void Start() {
        _gameController = GameControllerObj.GetComponent<GameController>();
    }

    private void OnEnable() {
        GameState.NumWallBouncesThisTurn = 0;
        setBallColor(GameConstants.BALL_START_COLOR);
    }

    private void OnCollisionEnter(Collision collision) {
        Transform colliderObjParent = collision.collider.gameObject.transform.parent;

        if (colliderObjParent != null && colliderObjParent.gameObject.tag == "Tunnel") {
            GameState.NumWallBouncesThisTurn++;

            if (GameState.NumWallBouncesThisTurn == GameConstants.NUM_BOUNCES_FOR_LOSS) {
                _gameController.EndRound(!GameState.PlayerHitLast);
            }
            else {
                float percentageOfBounceLimit = (float)GameState.NumWallBouncesThisTurn / GameConstants.NUM_BOUNCES_FOR_LOSS;
                setBallColor(Color.Lerp(
                    GameConstants.BALL_START_COLOR,
                    GameConstants.BOUNCE_LOSS_COLOR,
                    percentageOfBounceLimit));
            }            
        }        
    }

    public void HandleTurnChange() {
        GameState.NumWallBouncesThisTurn = 0;
        setBallColor(GameConstants.BALL_START_COLOR);
    }

    private void setBallColor(Color color) {
        _ballLight.color = color;
        _ballMaterial.SetColor("_EmissionColor", color);
        _ballMaterial.SetColor("_SpecColor", color);
    }
}
