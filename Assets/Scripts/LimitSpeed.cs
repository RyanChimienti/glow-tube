using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitSpeed : MonoBehaviour
{
    public float maxSpeed;
    public Rigidbody rb;

    private void Start() {
        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
}
