using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBallSpeed : MonoBehaviour
{
    private Rigidbody rb;

    private void Start() {
        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float desiredSpeed = GameConstants.INITIAL_BALL_SPEED
            + GameConstants.BALL_SPEED_INCREMENT * GameState.TurnNumber;
        rb.velocity = Vector3.Normalize(rb.velocity) * desiredSpeed; 
            
    }
}
