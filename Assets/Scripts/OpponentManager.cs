using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpponentManager : MonoBehaviour {
    [Tooltip("The ball that is being volleyed")]
    public GameObject Ball;

    /// <summary>
    /// The position the opponent moves to in between rounds.
    /// </summary>
    private Vector3 _readyPosition;
    
    /// <summary>
    /// The opponent's RigidBody.
    /// </summary>
    private Rigidbody _rb;

    /// <summary>
    /// True if the opponent is following the ball position, false if it's
    /// moving to ready position.
    /// </summary>
    private bool _followingBall;

    private void Awake() {
        _readyPosition = this.transform.position;
        _rb = this.GetComponent<Rigidbody>();
    }

    public void OnRoundStart() {
        _followingBall = true;
    }

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        _followingBall = false;
    }

    private void FixedUpdate() {
        if (_followingBall) {
            moveTowardsBall();
        }
        else {
            moveTowardsReadyPosition();
        }
    }

    private void moveTowardsReadyPosition() {
        float distanceToMove = GameConstants.OPPONENT_RESET_SPEED * Time.fixedDeltaTime;

        _rb.MovePosition(
            Vector3.MoveTowards(
                this.transform.position,
                _readyPosition,
                distanceToMove
            )
        );
    }

    private void moveTowardsBall() {
        Plane oppPlane = new Plane(new Vector3(0, 0, 1), this.transform.position);
        Vector3 targetLocation = oppPlane.ClosestPointOnPlane(Ball.transform.position);
        float distanceToMove = GameConstants.OPPONENT_PLAY_SPEED * Time.fixedDeltaTime;

        _rb.MovePosition(
            Vector3.MoveTowards(
                this.transform.position,
                targetLocation,
                distanceToMove
            )
        );
    }
}
