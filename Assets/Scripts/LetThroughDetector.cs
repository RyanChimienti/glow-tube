using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LetThroughEvent : UnityEvent<bool> { }

/// <summary>
/// Fires an event when the ball escapes on either side.
/// </summary>
public class LetThroughDetector : MonoBehaviour {
    [Header("The argument is true if the let through was on the player's side.")]
    [SerializeField]
    public LetThroughEvent LetThroughEvent = new LetThroughEvent();

    public Collider PlayerBallArea;
    public Collider OpponentBallArea;
    public GameObject Ball;
    public GameObject Tunnel;

    private Bounds _playerAreaBounds;
    private Bounds _opponentAreaBounds;
    private Bounds _tunnelBounds;

    private bool _ballInPlayerArea;
    private bool _ballInOpponentArea;

    /// <summary>
    /// Are we currently detecting let throughs?
    /// </summary>
    private bool _detecting;
    
    private void Start() {
        _playerAreaBounds = PlayerBallArea.bounds;
        _opponentAreaBounds = OpponentBallArea.bounds;
        _tunnelBounds = Utils.GetBoundingBox(Tunnel);
        _detecting = false;
    }

    public void OnRoundStart() {
        _detecting = true;
    }

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        _detecting = false;
    }

    private void FixedUpdate(){
        if (!_detecting) {
            return;
        }

        Vector3 ballPos = Ball.transform.position;
        bool ballNowInPlayerArea = _playerAreaBounds.Contains(ballPos);
        bool ballNowInOppArea = _opponentAreaBounds.Contains(ballPos);
        bool ballNowInTunnel = _tunnelBounds.Contains(ballPos);

        if (!ballNowInTunnel) {
            if (_ballInPlayerArea && !ballNowInPlayerArea) {
                LetThroughEvent.Invoke(true);
            }
            else if (_ballInOpponentArea && !ballNowInOppArea) {
                LetThroughEvent.Invoke(false);
            }
        }

        _ballInPlayerArea = ballNowInPlayerArea;
        _ballInOpponentArea = ballNowInOppArea;
    }
}
