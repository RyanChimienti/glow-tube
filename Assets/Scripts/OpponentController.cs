using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpponentController : MonoBehaviour
{
    [Tooltip("The object containing the GameController script")]
    public GameObject GameControllerObj;

    [Tooltip("The ball that is being volleyed")]
    public GameObject Ball;

    /// <summary>
    /// The position the opponent moves to in between rounds.
    /// </summary>
    private Vector3 readyPosition;
    private Rigidbody rb;

    void Start() {
        readyPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Ball") {
            GameControllerObj.GetComponent<GameController>().HandleBallHit(false);
        }
    }

    void FixedUpdate()
    {
        if (GameState.CurrentStatus != GameState.Status.PLAYING_ROUND) {
            moveTowardsReadyPosition();
        }
        else {
            moveTowardsBall();
        }
    }

    private void moveTowardsReadyPosition() {
        float distanceToMove = GameConstants.OPPONENT_RESET_SPEED * Time.fixedDeltaTime;

        rb.MovePosition(
            Vector3.MoveTowards(
                this.transform.position,
                readyPosition,
                distanceToMove
            )
        );
    }

    private void moveTowardsBall() {
        Plane oppPlane = new Plane(new Vector3(0, 0, 1), this.transform.position);
        Vector3 targetLocation = oppPlane.ClosestPointOnPlane(Ball.transform.position);
        float distanceToMove = GameConstants.OPPONENT_PLAY_SPEED * Time.fixedDeltaTime;

        rb.MovePosition(
            Vector3.MoveTowards(
                this.transform.position,
                targetLocation,
                distanceToMove
            )
        );
    }
}
