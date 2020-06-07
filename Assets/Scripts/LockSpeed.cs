using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSpeed : MonoBehaviour
{
    private Rigidbody rb;

    private void Start() {
        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.Normalize(rb.velocity) * GameConstants.INITIAL_BALL_SPEED;
    }
}
