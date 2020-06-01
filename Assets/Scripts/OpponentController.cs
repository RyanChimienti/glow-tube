using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpponentController : MonoBehaviour
{
    [Tooltip("The object containing the GameController script")]
    public GameObject GameControllerObj;

    /// <summary>
    /// The speed of the opponent while playing in units per second.
    /// </summary>
    private static float PLAY_SPEED = 0.5F;

    /// <summary>
    /// The speed of the opponent while returning to its start position in 
    /// units per second.
    /// </summary>
    private static float RESET_SPEED = 0.5F;

    /// <summary>
    /// The position the opponent moves to in between rounds.
    /// </summary>
    private Vector3 readyPosition;

    void Start() {
        readyPosition = this.transform.position;
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
        float distanceToMove = RESET_SPEED * Time.fixedDeltaTime;

        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            readyPosition,
            distanceToMove);
    }

    private void moveTowardsBall() {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Plane oppPlane = new Plane(new Vector3(0, 0, 1), this.transform.position);
        Vector3 targetLocation = oppPlane.ClosestPointOnPlane(ball.transform.position);
        float distanceToMove = PLAY_SPEED * Time.fixedDeltaTime;

        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            targetLocation,
            distanceToMove);
    }
}
