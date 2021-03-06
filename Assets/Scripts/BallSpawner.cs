﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Activates, positions, and launches the ball at the start of a round.
/// </summary>
public class BallSpawner : MonoBehaviour {
    public GameObject ball;

    /// <summary>
    /// The tunnel (to help the spawner position itself).
    /// </summary>
    public GameObject tunnel;

    /// <summary>
    /// The menus (to help the spawner position itself).
    /// </summary>
    public GameObject menus;

    private Rigidbody _ballRb;

    private SphereCollider _ballCollider;

    private void Start() {
        ball.SetActive(false);
        _ballRb = ball.GetComponent<Rigidbody>();
        _ballCollider = ball.GetComponent<SphereCollider>();        
    }

    public void OnRoundStart() {
        UpdatePosition();
        ball.transform.position = this.transform.position;
        _ballRb.velocity = Vector3.zero;
        ball.SetActive(true);

        // Disable the ball's collider for 3 physics steps to prevent a strange collision that
        // sometimes occurs between the ball and the paddle right after starting a round.
        // (Specifically, this collision was happening after some double hit losses.)
        _ballCollider.enabled = false;
        Invoke("EnableBallCollider", Time.fixedDeltaTime * 3);

        // Launch the ball after 1.5 seconds.
        Invoke("LaunchBall", 1.5f);
    }

    private void EnableBallCollider() {
        _ballCollider.enabled = true;
    }

    private void LaunchBall() {
        Vector3 ballDirection = new Vector3(0, 0, -GameConstants.INITIAL_BALL_SPEED);
        Vector3 noisyBallDirection = Utils.RotateRandomly(ballDirection, GameConstants.MAX_BALL_START_ANGLE);
        _ballRb.velocity = noisyBallDirection;        
    }

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        // If the round somehow ends before the ball is launched 
        // (which shouldn't be possible), cancel the upcoming launch.
        CancelInvoke();
    }

    /// <summary>
    /// Places the spawner where it should go, based on the tunnel position
    /// and menu position.
    /// </summary>
    private void UpdatePosition() {
        Vector3 tunnelCenter = Utils.GetBoundingBox(tunnel).center;

        float xPos = tunnelCenter.x;
        float yPos = tunnelCenter.y;
        float zPos = menus.transform.position.z;
        this.transform.position = new Vector3(xPos, yPos, zPos);
    }
}
